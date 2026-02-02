(function ($) { //PRINCIPIO COMPONENTE BOOTSTRAP-SELECT
    "use strict";

    function icontains(haystack, needle) {
        return haystack.toUpperCase().indexOf(needle.toUpperCase()) > -1
    }

    function normalizeToBase(text) {
        var rExps = [{
            re: /[\xC0-\xC6]/g,
            ch: "A"
        }, {
            re: /[\xE0-\xE6]/g,
            ch: "a"
        }, {
            re: /[\xC8-\xCB]/g,
            ch: "E"
        }, {
            re: /[\xE8-\xEB]/g,
            ch: "e"
        }, {
            re: /[\xCC-\xCF]/g,
            ch: "I"
        }, {
            re: /[\xEC-\xEF]/g,
            ch: "i"
        }, {
            re: /[\xD2-\xD6]/g,
            ch: "O"
        }, {
            re: /[\xF2-\xF6]/g,
            ch: "o"
        }, {
            re: /[\xD9-\xDC]/g,
            ch: "U"
        }, {
            re: /[\xF9-\xFC]/g,
            ch: "u"
        }, {
            re: /[\xC7-\xE7]/g,
            ch: "c"
        }, {
            re: /[\xD1]/g,
            ch: "N"
        }, {
            re: /[\xF1]/g,
            ch: "n"
        }];
        return $.each(rExps, function () {
            text = text.replace(this.re, this.ch)
        }), text
    }

    function htmlEscape(html) {
        var escapeMap = {
            "&": "&amp;",
            "<": "&lt;",
            ">": "&gt;",
            '"': "&quot;",
            "'": "&#x27;",
            "`": "&#x60;"
        };
        Object.keys = Object.keys || function (o, k, r) {
            r = [];
            for (k in o) r.hasOwnProperty.call(o, k) && r.push(k);
            return r
        };
        var source = "(?:" + Object.keys(escapeMap).join("|") + ")",
            testRegexp = new RegExp(source),
            replaceRegexp = new RegExp(source, "g"),
            string = null == html ? "" : "" + html;
        return testRegexp.test(string) ? string.replace(replaceRegexp, function (match) {
            return escapeMap[match]
        }) : string
    }

    function Plugin(option, event) {
        var args = arguments,
            _option = option,
            option = args[0],
            event = args[1];
        [].shift.apply(args), "undefined" == typeof option && (option = _option);
        var value, chain = this.each(function () {
            var $this = $(this);
            if ($this.is("select")) {
                var data = $this.data("selectpicker"),
                    options = "object" == typeof option && option;
                if (data) {
                    if (options)
                        for (var i in options) options.hasOwnProperty(i) && (data.options[i] = options[i])
                } else {
                    var config = $.extend({}, Selectpicker.DEFAULTS, $.fn.selectpicker.defaults || {}, $this.data(), options);
                    $this.data("selectpicker", data = new Selectpicker(this, config, event))
                }
                "string" == typeof option && (value = data[option] instanceof Function ? data[option].apply(data, args) : data.options[option])
            }
        });
        return "undefined" != typeof value ? value : chain
    }
    $.expr[":"].icontains = function (obj, index, meta) {
        return icontains($(obj).text(), meta[3])
    }, $.expr[":"].aicontains = function (obj, index, meta) {
        return icontains($(obj).data("normalizedText") || $(obj).text(), meta[3])
    };
    var Selectpicker = function (element, options, e) {
        e && (e.stopPropagation(), e.preventDefault()), this.$element = $(element), this.$newElement = null, this.$button = null, this.$menu = null, this.$lis = null, this.options = options, null === this.options.title && (this.options.title = this.$element.attr("title")), this.val = Selectpicker.prototype.val, this.render = Selectpicker.prototype.render, this.refresh = Selectpicker.prototype.refresh, this.setStyle = Selectpicker.prototype.setStyle, this.selectAll = Selectpicker.prototype.selectAll, this.deselectAll = Selectpicker.prototype.deselectAll, this.destroy = Selectpicker.prototype.remove, this.remove = Selectpicker.prototype.remove, this.show = Selectpicker.prototype.show, this.hide = Selectpicker.prototype.hide, this.init()
    };
    Selectpicker.VERSION = "1.6.3", Selectpicker.DEFAULTS = {
        noneSelectedText: "Nothing selected",
        noneResultsText: "No results match",
        countSelectedText: function (numSelected) {
            return 1 == numSelected ? "{0} item selected" : "{0} items selected"
        },
        maxOptionsText: function (numAll, numGroup) {
            var arr = [];
            return arr[0] = 1 == numAll ? "Limit reached ({n} item max)" : "Limit reached ({n} items max)", arr[1] = 1 == numGroup ? "Group limit reached ({n} item max)" : "Group limit reached ({n} items max)", arr
        },
        selectAllText: "Select All",
        deselectAllText: "Deselect All",
        multipleSeparator: ", ",
        style: "btn-default",
        size: "auto",
        title: null,
        selectedTextFormat: "values",
        width: !1,
        container: !1,
        hideDisabled: !1,
        showSubtext: !1,
        showIcon: !0,
        showContent: !0,
        dropupAuto: !0,
        header: !1,
        liveSearch: !1,
        actionsBox: !1,
        iconBase: "glyphicon",
        tickIcon: "glyphicon-ok",
        maxOptions: !1,
        mobile: !1,
        selectOnTab: !1,
        dropdownAlignRight: !1,
        searchAccentInsensitive: !1
    }, Selectpicker.prototype = {
        constructor: Selectpicker,
        init: function () {
            var that = this,
                id = this.$element.attr("id");
            this.$element.hide(), this.multiple = this.$element.prop("multiple"), this.autofocus = this.$element.prop("autofocus"), this.$newElement = this.createView(), this.$element.after(this.$newElement), this.$menu = this.$newElement.find("> .dropdown-menu"), this.$button = this.$newElement.find("> button"), this.$searchbox = this.$newElement.find("input"), this.options.dropdownAlignRight && this.$menu.addClass("dropdown-menu-right"), "undefined" != typeof id && (this.$button.attr("data-id", id), $('label[for="' + id + '"]').click(function (e) {
                e.preventDefault(), that.$button.focus()
            })), this.checkDisabled(), this.clickListener(), this.options.liveSearch && this.liveSearchListener(), this.render(), this.liHeight(), this.setStyle(), this.setWidth(), this.options.container && this.selectPosition(), this.$menu.data("this", this), this.$newElement.data("this", this), this.options.mobile && this.mobile()
        },
        createDropdown: function () {
            var multiple = this.multiple ? " show-tick" : "",
                inputGroup = this.$element.parent().hasClass("input-group") ? " input-group-btn" : "",
                autofocus = this.autofocus ? " autofocus" : "",
                btnSize = this.$element.parents().hasClass("form-group-lg") ? " btn-lg" : this.$element.parents().hasClass("form-group-sm") ? " btn-sm" : "",
                header = this.options.header ? '<div class="popover-title"><button type="button" class="close" aria-hidden="true">&times;</button>' + this.options.header + "</div>" : "",
                searchbox = this.options.liveSearch ? '<div class="bs-searchbox"><input type="text" class="input-block-level form-control" autocomplete="off" /></div>' : "",
                actionsbox = this.options.actionsBox ? '<div class="bs-actionsbox"><div class="btn-group btn-block"><button class="actions-btn bs-select-all btn btn-sm btn-default">' + this.options.selectAllText + '</button><button class="actions-btn bs-deselect-all btn btn-sm btn-default">' + this.options.deselectAllText + "</button></div></div>" : "",
                drop = '<div class="btn-group bootstrap-select' + multiple + inputGroup + '"><button type="button" class="btn dropdown-toggle selectpicker' + btnSize + '" data-toggle="dropdown"' + autofocus + '><span class="filter-option pull-left"></span>&nbsp;<span class="caret"></span></button><div class="dropdown-menu open">' + header + searchbox + actionsbox + '<ul class="dropdown-menu inner selectpicker" role="menu"></ul></div></div>';
            return $(drop)
        },
        createView: function () {
            var $drop = this.createDropdown(),
                $li = this.createLi();
            return $drop.find("ul").append($li), $drop
        },
        reloadLi: function () {
            this.destroyLi();
            var $li = this.createLi();
            this.$menu.find("ul").append($li)
        },
        destroyLi: function () {
            this.$menu.find("li").remove()
        },
        createLi: function () {
            var that = this,
                _li = [],
                optID = 0,
                generateLI = function (content, index, classes) {
                    return "<li" + ("undefined" != typeof classes ? ' class="' + classes + '"' : "") + ("undefined" != typeof index | null === index ? ' data-original-index="' + index + '"' : "") + ">" + content + "</li>"
                },
                generateA = function (text, classes, inline, optgroup) {
                    var normText = normalizeToBase(htmlEscape(text));
                    return '<a tabindex="0"' + ("undefined" != typeof classes ? ' class="' + classes + '"' : "") + ("undefined" != typeof inline ? ' style="' + inline + '"' : "") + ("undefined" != typeof optgroup ? 'data-optgroup="' + optgroup + '"' : "") + ' data-normalized-text="' + normText + '">' + text + '<span class="' + that.options.iconBase + " " + that.options.tickIcon + ' check-mark"></span></a>'
                };
            return this.$element.find("option").each(function () {
                var $this = $(this),
                    optionClass = $this.attr("class") || "",
                    inline = $this.attr("style"),
                    text = $this.data("content") ? $this.data("content") : $this.html(),
                    subtext = "undefined" != typeof $this.data("subtext") ? '<small class="muted text-muted">' + $this.data("subtext") + "</small>" : "",
                    icon = "undefined" != typeof $this.data("icon") ? '<span class="' + that.options.iconBase + " " + $this.data("icon") + '"></span> ' : "",
                    isDisabled = $this.is(":disabled") || $this.parent().is(":disabled"),
                    index = $this[0].index;
                if ("" !== icon && isDisabled && (icon = "<span>" + icon + "</span>"), $this.data("content") || (text = icon + '<span class="text">' + text + subtext + "</span>"), !that.options.hideDisabled || !isDisabled)
                    if ($this.parent().is("optgroup") && $this.data("divider") !== !0) {
                        if (0 === $this.index()) {
                            optID += 1;
                            var label = $this.parent().attr("label"),
                                labelSubtext = "undefined" != typeof $this.parent().data("subtext") ? '<small class="muted text-muted">' + $this.parent().data("subtext") + "</small>" : "",
                                labelIcon = $this.parent().data("icon") ? '<span class="' + that.options.iconBase + " " + $this.parent().data("icon") + '"></span> ' : "";
                            label = labelIcon + '<span class="text">' + label + labelSubtext + "</span>", 0 !== index && _li.length > 0 && _li.push(generateLI("", null, "divider")), _li.push(generateLI(label, null, "dropdown-header"))
                        }
                        _li.push(generateLI(generateA(text, "opt " + optionClass, inline, optID), index))
                    } else _li.push($this.data("divider") === !0 ? generateLI("", index, "divider") : $this.data("hidden") === !0 ? generateLI(generateA(text, optionClass, inline), index, "hide is-hidden") : generateLI(generateA(text, optionClass, inline), index))
            }), this.multiple || 0 !== this.$element.find("option:selected").length || this.options.title || this.$element.find("option").eq(0).prop("selected", !0).attr("selected", "selected"), $(_li.join(""))
        },
        findLis: function () {
            return null == this.$lis && (this.$lis = this.$menu.find("li")), this.$lis
        },
        render: function (updateLi) {
            var that = this;
            updateLi !== !1 && this.$element.find("option").each(function (index) {
                that.setDisabled(index, $(this).is(":disabled") || $(this).parent().is(":disabled")), that.setSelected(index, $(this).is(":selected"))
            }), this.tabIndex();
            var notDisabled = this.options.hideDisabled ? ":not([disabled])" : "",
                selectedItems = this.$element.find("option:selected" + notDisabled).map(function () {
                    var subtext, $this = $(this),
                        icon = $this.data("icon") && that.options.showIcon ? '<i class="' + that.options.iconBase + " " + $this.data("icon") + '"></i> ' : "";
                    return subtext = that.options.showSubtext && $this.attr("data-subtext") && !that.multiple ? ' <small class="muted text-muted">' + $this.data("subtext") + "</small>" : "", $this.data("content") && that.options.showContent ? $this.data("content") : "undefined" != typeof $this.attr("title") ? $this.attr("title") : icon + $this.html() + subtext
                }).toArray(),
                title = this.multiple ? selectedItems.join(this.options.multipleSeparator) : selectedItems[0];
            if (this.multiple && this.options.selectedTextFormat.indexOf("count") > -1) {
                var max = this.options.selectedTextFormat.split(">");
                if (max.length > 1 && selectedItems.length > max[1] || 1 == max.length && selectedItems.length >= 2) {
                    notDisabled = this.options.hideDisabled ? ", [disabled]" : "";
                    var totalCount = this.$element.find("option").not('[data-divider="true"], [data-hidden="true"]' + notDisabled).length,
                        tr8nText = "function" == typeof this.options.countSelectedText ? this.options.countSelectedText(selectedItems.length, totalCount) : this.options.countSelectedText;
                    title = tr8nText.replace("{0}", selectedItems.length.toString()).replace("{1}", totalCount.toString())
                }
            }
            this.options.title = this.$element.attr("title"), "static" == this.options.selectedTextFormat && (title = this.options.title), title || (title = "undefined" != typeof this.options.title ? this.options.title : this.options.noneSelectedText), this.$button.attr("title", htmlEscape(title)), this.$newElement.find(".filter-option").html(title)
        },
        setStyle: function (style, status) {
            this.$element.attr("class") && this.$newElement.addClass(this.$element.attr("class").replace(/selectpicker|mobile-device|validate\[.*\]/gi, ""));
            var buttonClass = style ? style : this.options.style;
            "add" == status ? this.$button.addClass(buttonClass) : "remove" == status ? this.$button.removeClass(buttonClass) : (this.$button.removeClass(this.options.style), this.$button.addClass(buttonClass))
        },
        liHeight: function () {
            if (this.options.size !== !1) {
                var $selectClone = this.$menu.parent().clone().find("> .dropdown-toggle").prop("autofocus", !1).end().appendTo("body"),
                    $menuClone = $selectClone.addClass("open").find("> .dropdown-menu"),
                    liHeight = $menuClone.find("li").not(".divider").not(".dropdown-header").filter(":visible").children("a").outerHeight(),
                    headerHeight = this.options.header ? $menuClone.find(".popover-title").outerHeight() : 0,
                    searchHeight = this.options.liveSearch ? $menuClone.find(".bs-searchbox").outerHeight() : 0,
                    actionsHeight = this.options.actionsBox ? $menuClone.find(".bs-actionsbox").outerHeight() : 0;
                $selectClone.remove(), this.$newElement.data("liHeight", liHeight).data("headerHeight", headerHeight).data("searchHeight", searchHeight).data("actionsHeight", actionsHeight)
            }
        },
        setSize: function () {
            this.findLis();
            var menuHeight, selectOffsetTop, selectOffsetBot, that = this,
                menu = this.$menu,
                menuInner = menu.find(".inner"),
                selectHeight = this.$newElement.outerHeight(),
                liHeight = this.$newElement.data("liHeight"),
                headerHeight = this.$newElement.data("headerHeight"),
                searchHeight = this.$newElement.data("searchHeight"),
                actionsHeight = this.$newElement.data("actionsHeight"),
                divHeight = this.$lis.filter(".divider").outerHeight(!0),
                menuPadding = parseInt(menu.css("padding-top")) + parseInt(menu.css("padding-bottom")) + parseInt(menu.css("border-top-width")) + parseInt(menu.css("border-bottom-width")),
                notDisabled = this.options.hideDisabled ? ", .disabled" : "",
                $window = $(window),
                menuExtras = menuPadding + parseInt(menu.css("margin-top")) + parseInt(menu.css("margin-bottom")) + 2,
                posVert = function () {
                    selectOffsetTop = that.$newElement.offset().top - $window.scrollTop(), selectOffsetBot = $window.height() - selectOffsetTop - selectHeight
                };
            if (posVert(), this.options.header && menu.css("padding-top", 0), "auto" == this.options.size) {
                var getSize = function () {
                    var minHeight, lisVis = that.$lis.not(".hide");
                    posVert(), menuHeight = selectOffsetBot - menuExtras, that.options.dropupAuto && that.$newElement.toggleClass("dropup", selectOffsetTop > selectOffsetBot && menuHeight - menuExtras < menu.height()), that.$newElement.hasClass("dropup") && (menuHeight = selectOffsetTop - menuExtras), minHeight = lisVis.length + lisVis.filter(".dropdown-header").length > 3 ? 3 * liHeight + menuExtras - 2 : 0, menu.css({
                        "max-height": menuHeight + "px",
                        overflow: "hidden",
                        "min-height": minHeight + headerHeight + searchHeight + actionsHeight + "px"
                    }), menuInner.css({
                        "max-height": menuHeight - headerHeight - searchHeight - actionsHeight - menuPadding + "px",
                        "overflow-y": "auto",
                        "min-height": Math.max(minHeight - menuPadding, 0) + "px"
                    })
                };
                getSize(), this.$searchbox.off("input.getSize propertychange.getSize").on("input.getSize propertychange.getSize", getSize), $(window).off("resize.getSize").on("resize.getSize", getSize), $(window).off("scroll.getSize").on("scroll.getSize", getSize)
            } else if (this.options.size && "auto" != this.options.size && menu.find("li" + notDisabled).length > this.options.size) {
                var optIndex = this.$lis.not(".divider" + notDisabled).find(" > *").slice(0, this.options.size).last().parent().index(),
                    divLength = this.$lis.slice(0, optIndex + 1).filter(".divider").length;
                menuHeight = liHeight * this.options.size + divLength * divHeight + menuPadding, that.options.dropupAuto && this.$newElement.toggleClass("dropup", selectOffsetTop > selectOffsetBot && menuHeight < menu.height()), menu.css({
                    "max-height": menuHeight + headerHeight + searchHeight + actionsHeight + "px",
                    overflow: "hidden"
                }), menuInner.css({
                    "max-height": menuHeight - menuPadding + "px",
                    "overflow-y": "auto"
                })
            }
        },
        setWidth: function () {
            if ("auto" == this.options.width) {
                this.$menu.css("min-width", "0");
                var selectClone = this.$newElement.clone().appendTo("body"),
                    ulWidth = selectClone.find("> .dropdown-menu").css("width"),
                    btnWidth = selectClone.css("width", "auto").find("> button").css("width");
                selectClone.remove(), this.$newElement.css("width", Math.max(parseInt(ulWidth), parseInt(btnWidth)) + "px")
            } else "fit" == this.options.width ? (this.$menu.css("min-width", ""), this.$newElement.css("width", "").addClass("fit-width")) : this.options.width ? (this.$menu.css("min-width", ""), this.$newElement.css("width", this.options.width)) : (this.$menu.css("min-width", ""), this.$newElement.css("width", ""));
            this.$newElement.hasClass("fit-width") && "fit" !== this.options.width && this.$newElement.removeClass("fit-width")
        },
        selectPosition: function () {
            var pos, actualHeight, that = this,
                drop = "<div />",
                $drop = $(drop),
                getPlacement = function ($element) {
                    $drop.addClass($element.attr("class").replace(/form-control/gi, "")).toggleClass("dropup", $element.hasClass("dropup")), pos = $element.offset(), actualHeight = $element.hasClass("dropup") ? 0 : $element[0].offsetHeight, $drop.css({
                        top: pos.top + actualHeight,
                        left: pos.left,
                        width: $element[0].offsetWidth,
                        position: "absolute"
                    })
                };
            this.$newElement.on("click", function () {
                that.isDisabled() || (getPlacement($(this)), $drop.appendTo(that.options.container), $drop.toggleClass("open", !$(this).hasClass("open")), $drop.append(that.$menu))
            }), $(window).resize(function () {
                getPlacement(that.$newElement)
            }), $(window).on("scroll", function () {
                getPlacement(that.$newElement)
            }), $("html").on("click", function (e) {
                $(e.target).closest(that.$newElement).length < 1 && $drop.removeClass("open")
            })
        },
        setSelected: function (index, selected) {
            this.findLis(), this.$lis.filter('[data-original-index="' + index + '"]').toggleClass("selected", selected)
        },
        setDisabled: function (index, disabled) {
            this.findLis(), disabled ? this.$lis.filter('[data-original-index="' + index + '"]').addClass("disabled").find("a").attr("href", "#").attr("tabindex", -1) : this.$lis.filter('[data-original-index="' + index + '"]').removeClass("disabled").find("a").removeAttr("href").attr("tabindex", 0)
        },
        isDisabled: function () {
            return this.$element.is(":disabled")
        },
        checkDisabled: function () {
            var that = this;
            this.isDisabled() ? this.$button.addClass("disabled").attr("tabindex", -1) : (this.$button.hasClass("disabled") && this.$button.removeClass("disabled"), -1 == this.$button.attr("tabindex") && (this.$element.data("tabindex") || this.$button.removeAttr("tabindex"))), this.$button.click(function () {
                return !that.isDisabled()
            })
        },
        tabIndex: function () {
            this.$element.is("[tabindex]") && (this.$element.data("tabindex", this.$element.attr("tabindex")), this.$button.attr("tabindex", this.$element.data("tabindex")))
        },
        clickListener: function () {
            var that = this;
            this.$newElement.on("touchstart.dropdown", ".dropdown-menu", function (e) {
                e.stopPropagation()
            }), this.$newElement.on("click", function () {
                that.setSize(), that.options.liveSearch || that.multiple || setTimeout(function () {
                    that.$menu.find(".selected a").focus()
                }, 10)
            }), this.$menu.on("click", "li a", function (e) {
                var $this = $(this),
                    clickedIndex = $this.parent().data("originalIndex"),
                    prevValue = that.$element.val(),
                    prevIndex = that.$element.prop("selectedIndex");
                if (that.multiple && e.stopPropagation(), e.preventDefault(), !that.isDisabled() && !$this.parent().hasClass("disabled")) {
                    var $options = that.$element.find("option"),
                        $option = $options.eq(clickedIndex),
                        state = $option.prop("selected"),
                        $optgroup = $option.parent("optgroup"),
                        maxOptions = that.options.maxOptions,
                        maxOptionsGrp = $optgroup.data("maxOptions") || !1;
                    if (that.multiple) {
                        if ($option.prop("selected", !state), that.setSelected(clickedIndex, !state), $this.blur(), maxOptions !== !1 || maxOptionsGrp !== !1) {
                            var maxReached = maxOptions < $options.filter(":selected").length,
                                maxReachedGrp = maxOptionsGrp < $optgroup.find("option:selected").length;
                            if (maxOptions && maxReached || maxOptionsGrp && maxReachedGrp)
                                if (maxOptions && 1 == maxOptions) $options.prop("selected", !1), $option.prop("selected", !0), that.$menu.find(".selected").removeClass("selected"), that.setSelected(clickedIndex, !0);
                                else if (maxOptionsGrp && 1 == maxOptionsGrp) {
                                    $optgroup.find("option:selected").prop("selected", !1), $option.prop("selected", !0);
                                    var optgroupID = $this.data("optgroup");
                                    that.$menu.find(".selected").has('a[data-optgroup="' + optgroupID + '"]').removeClass("selected"), that.setSelected(clickedIndex, !0)
                                } else {
                                    var maxOptionsArr = "function" == typeof that.options.maxOptionsText ? that.options.maxOptionsText(maxOptions, maxOptionsGrp) : that.options.maxOptionsText,
                                        maxTxt = maxOptionsArr[0].replace("{n}", maxOptions),
                                        maxTxtGrp = maxOptionsArr[1].replace("{n}", maxOptionsGrp),
                                        $notify = $('<div class="notify"></div>');
                                    maxOptionsArr[2] && (maxTxt = maxTxt.replace("{var}", maxOptionsArr[2][maxOptions > 1 ? 0 : 1]), maxTxtGrp = maxTxtGrp.replace("{var}", maxOptionsArr[2][maxOptionsGrp > 1 ? 0 : 1])), $option.prop("selected", !1), that.$menu.append($notify), maxOptions && maxReached && ($notify.append($("<div>" + maxTxt + "</div>")), that.$element.trigger("maxReached.bs.select")), maxOptionsGrp && maxReachedGrp && ($notify.append($("<div>" + maxTxtGrp + "</div>")), that.$element.trigger("maxReachedGrp.bs.select")), setTimeout(function () {
                                        that.setSelected(clickedIndex, !1)
                                    }, 10), $notify.delay(750).fadeOut(300, function () {
                                        $(this).remove()
                                    })
                                }
                        }
                    } else $options.prop("selected", !1), $option.prop("selected", !0), that.$menu.find(".selected").removeClass("selected"), that.setSelected(clickedIndex, !0);
                    that.multiple ? that.options.liveSearch && that.$searchbox.focus() : that.$button.focus(), (prevValue != that.$element.val() && that.multiple || prevIndex != that.$element.prop("selectedIndex") && !that.multiple) && that.$element.change()
                }
            }), this.$menu.on("click", "li.disabled a, .popover-title, .popover-title :not(.close)", function (e) {
                e.target == this && (e.preventDefault(), e.stopPropagation(), that.options.liveSearch ? that.$searchbox.focus() : that.$button.focus())
            }), this.$menu.on("click", "li.divider, li.dropdown-header", function (e) {
                e.preventDefault(), e.stopPropagation(), that.options.liveSearch ? that.$searchbox.focus() : that.$button.focus()
            }), this.$menu.on("click", ".popover-title .close", function () {
                that.$button.focus()
            }), this.$searchbox.on("click", function (e) {
                e.stopPropagation()
            }), this.$menu.on("click", ".actions-btn", function (e) {
                that.options.liveSearch ? that.$searchbox.focus() : that.$button.focus(), e.preventDefault(), e.stopPropagation(), $(this).is(".bs-select-all") ? that.selectAll() : that.deselectAll(), that.$element.change()
            }), this.$element.change(function () {
                that.render(!1)
            })
        },
        liveSearchListener: function () {
            var that = this,
                no_results = $('<li class="no-results"></li>');
            this.$newElement.on("click.dropdown.data-api touchstart.dropdown.data-api", function () {
                that.$menu.find(".active").removeClass("active"), that.$searchbox.val() && (that.$searchbox.val(""), that.$lis.not(".is-hidden").removeClass("hide"), no_results.parent().length && no_results.remove()), that.multiple || that.$menu.find(".selected").addClass("active"), setTimeout(function () {
                    that.$searchbox.focus()
                }, 10)
            }), this.$searchbox.on("click.dropdown.data-api focus.dropdown.data-api touchend.dropdown.data-api", function (e) {
                e.stopPropagation()
            }), this.$searchbox.on("input propertychange", function () {
                that.$searchbox.val() ? (that.options.searchAccentInsensitive ? that.$lis.not(".is-hidden").removeClass("hide").find("a").not(":aicontains(" + normalizeToBase(that.$searchbox.val()) + ")").parent().addClass("hide") : that.$lis.not(".is-hidden").removeClass("hide").find("a").not(":icontains(" + that.$searchbox.val() + ")").parent().addClass("hide"), that.$menu.find("li").filter(":visible:not(.no-results)").length ? no_results.parent().length && no_results.remove() : (no_results.parent().length && no_results.remove(), no_results.html(that.options.noneResultsText + ' "' + htmlEscape(that.$searchbox.val()) + '"').show(), that.$menu.find("li").last().after(no_results))) : (that.$lis.not(".is-hidden").removeClass("hide"), no_results.parent().length && no_results.remove()), that.$menu.find("li.active").removeClass("active"), that.$menu.find("li").filter(":visible:not(.divider)").eq(0).addClass("active").find("a").focus(), $(this).focus()
            })
        },
        val: function (value) {
            return "undefined" != typeof value ? (this.$element.val(value), this.render(), this.$element) : this.$element.val()
        },
        selectAll: function () {
            this.findLis(), this.$lis.not(".divider").not(".disabled").not(".selected").filter(":visible").find("a").click()
        },
        deselectAll: function () {
            this.findLis(), this.$lis.not(".divider").not(".disabled").filter(".selected").filter(":visible").find("a").click()
        },
        keydown: function (e) {
            var $items, index, next, first, last, prev, nextPrev, prevIndex, isActive, $this = $(this),
                $parent = $this.is("input") ? $this.parent().parent() : $this.parent(),
                that = $parent.data("this"),
                keyCodeMap = {
                    32: " ",
                    48: "0",
                    49: "1",
                    50: "2",
                    51: "3",
                    52: "4",
                    53: "5",
                    54: "6",
                    55: "7",
                    56: "8",
                    57: "9",
                    59: ";",
                    65: "a",
                    66: "b",
                    67: "c",
                    68: "d",
                    69: "e",
                    70: "f",
                    71: "g",
                    72: "h",
                    73: "i",
                    74: "j",
                    75: "k",
                    76: "l",
                    77: "m",
                    78: "n",
                    79: "o",
                    80: "p",
                    81: "q",
                    82: "r",
                    83: "s",
                    84: "t",
                    85: "u",
                    86: "v",
                    87: "w",
                    88: "x",
                    89: "y",
                    90: "z",
                    96: "0",
                    97: "1",
                    98: "2",
                    99: "3",
                    100: "4",
                    101: "5",
                    102: "6",
                    103: "7",
                    104: "8",
                    105: "9"
                };
            if (that.options.liveSearch && ($parent = $this.parent().parent()), that.options.container && ($parent = that.$menu), $items = $("[role=menu] li a", $parent), isActive = that.$menu.parent().hasClass("open"), !isActive && /([0-9]|[A-z])/.test(String.fromCharCode(e.keyCode)) && (that.options.container ? that.$newElement.trigger("click") : (that.setSize(), that.$menu.parent().addClass("open"), isActive = !0), that.$searchbox.focus()), that.options.liveSearch && (/(^9$|27)/.test(e.keyCode.toString(10)) && isActive && 0 === that.$menu.find(".active").length && (e.preventDefault(), that.$menu.parent().removeClass("open"), that.$button.focus()), $items = $("[role=menu] li:not(.divider):not(.dropdown-header):visible", $parent), $this.val() || /(38|40)/.test(e.keyCode.toString(10)) || 0 === $items.filter(".active").length && ($items = that.$newElement.find("li").filter(that.options.searchAccentInsensitive ? ":aicontains(" + normalizeToBase(keyCodeMap[e.keyCode]) + ")" : ":icontains(" + keyCodeMap[e.keyCode] + ")"))), $items.length) {
                if (/(38|40)/.test(e.keyCode.toString(10))) index = $items.index($items.filter(":focus")), first = $items.parent(":not(.disabled):visible").first().index(), last = $items.parent(":not(.disabled):visible").last().index(), next = $items.eq(index).parent().nextAll(":not(.disabled):visible").eq(0).index(), prev = $items.eq(index).parent().prevAll(":not(.disabled):visible").eq(0).index(), nextPrev = $items.eq(next).parent().prevAll(":not(.disabled):visible").eq(0).index(), that.options.liveSearch && ($items.each(function (i) {
                    $(this).is(":not(.disabled)") && $(this).data("index", i)
                }), index = $items.index($items.filter(".active")), first = $items.filter(":not(.disabled):visible").first().data("index"), last = $items.filter(":not(.disabled):visible").last().data("index"), next = $items.eq(index).nextAll(":not(.disabled):visible").eq(0).data("index"), prev = $items.eq(index).prevAll(":not(.disabled):visible").eq(0).data("index"), nextPrev = $items.eq(next).prevAll(":not(.disabled):visible").eq(0).data("index")), prevIndex = $this.data("prevIndex"), 38 == e.keyCode && (that.options.liveSearch && (index -= 1), index != nextPrev && index > prev && (index = prev), first > index && (index = first), index == prevIndex && (index = last)), 40 == e.keyCode && (that.options.liveSearch && (index += 1), -1 == index && (index = 0), index != nextPrev && next > index && (index = next), index > last && (index = last), index == prevIndex && (index = first)), $this.data("prevIndex", index), that.options.liveSearch ? (e.preventDefault(), $this.is(".dropdown-toggle") || ($items.removeClass("active"), $items.eq(index).addClass("active").find("a").focus(), $this.focus())) : $items.eq(index).focus();
                else if (!$this.is("input")) {
                    var count, prevKey, keyIndex = [];
                    $items.each(function () {
                        $(this).parent().is(":not(.disabled)") && $.trim($(this).text().toLowerCase()).substring(0, 1) == keyCodeMap[e.keyCode] && keyIndex.push($(this).parent().index())
                    }), count = $(document).data("keycount"), count++, $(document).data("keycount", count), prevKey = $.trim($(":focus").text().toLowerCase()).substring(0, 1), prevKey != keyCodeMap[e.keyCode] ? (count = 1, $(document).data("keycount", count)) : count >= keyIndex.length && ($(document).data("keycount", 0), count > keyIndex.length && (count = 1)), $items.eq(keyIndex[count - 1]).focus()
                } (/(13|32)/.test(e.keyCode.toString(10)) || /(^9$)/.test(e.keyCode.toString(10)) && that.options.selectOnTab) && isActive && (/(32)/.test(e.keyCode.toString(10)) || e.preventDefault(), that.options.liveSearch ? /(32)/.test(e.keyCode.toString(10)) || (that.$menu.find(".active a").click(), $this.focus()) : $(":focus").click(), $(document).data("keycount", 0)), (/(^9$|27)/.test(e.keyCode.toString(10)) && isActive && (that.multiple || that.options.liveSearch) || /(27)/.test(e.keyCode.toString(10)) && !isActive) && (that.$menu.parent().removeClass("open"), that.$button.focus())
            }
        },
        mobile: function () {
            this.$element.addClass("mobile-device").appendTo(this.$newElement), this.options.container && this.$menu.hide()
        },
        refresh: function () {
            this.$lis = null, this.reloadLi(), this.render(), this.setWidth(), this.setStyle(), this.checkDisabled(), this.liHeight()
        },
        update: function () {
            this.reloadLi(), this.setWidth(), this.setStyle(), this.checkDisabled(), this.liHeight()
        },
        hide: function () {
            this.$newElement.hide()
        },
        show: function () {
            this.$newElement.show()
        },
        remove: function () {
            this.$newElement.remove(), this.$element.remove()
        }
    };
    var old = $.fn.selectpicker;
    $.fn.selectpicker = Plugin, $.fn.selectpicker.Constructor = Selectpicker, $.fn.selectpicker.noConflict = function () {
        return $.fn.selectpicker = old, this
    }, $(document).data("keycount", 0).on("keydown", ".bootstrap-select [data-toggle=dropdown], .bootstrap-select [role=menu], .bs-searchbox input", Selectpicker.prototype.keydown).on("focusin.modal", ".bootstrap-select [data-toggle=dropdown], .bootstrap-select [role=menu], .bs-searchbox input", function (e) {
        e.stopPropagation()
    }), $(window).on("load.bs.select.data-api", function () {
        $(".selectpicker").each(function () {
            var $selectpicker = $(this);
            Plugin.call($selectpicker, $selectpicker.data())
        })
    })
})(jQuery);