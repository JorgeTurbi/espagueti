! function(exports, global) {
    function closeCanvas() {
        $("body").hasClass("in") && ($(".off-canvas").removeClass("out"), $("body").removeClass("in").addClass("out"), $("html").removeClass("in").addClass("out"), $(".canvas-wrap").hide(), setTimeout(function() {
            $("body").removeClass("out left right"), $("html").removeClass("out left right")
        }, 500))
    }

    function initAsync() {
        windowWidth >= xsBreak && $(".selectpicker").selectpicker(), $(".dropdown-toggle").dropdown(), $(".js-input-slider").slider(), $(".js-datepicker").datepicker({
            autoclose: !0,
            locale: "es"
        }), $('[data-toggle="tooltip"]').tooltip({
            animated: "fade",
            html: !0
        }), $('[data-toggle="popover"]').popover({
            html: !0
        }), $(".nav-tabs.tab-drop").tabdrop(), $("input,select,textarea").not("[type=submit]").jqBootstrapValidation({
            filter: function() {
                return $(this).is(":visible")
            }
        })
    }
    global["true"] = exports, + function($) {
            "use strict";

            function transitionEnd() {
                var el = document.createElement("bootstrap"),
                    transEndEventNames = {
                        WebkitTransition: "webkitTransitionEnd",
                        MozTransition: "transitionend",
                        OTransition: "oTransitionEnd otransitionend",
                        transition: "transitionend"
                    };
                for (var name in transEndEventNames)
                    if (void 0 !== el.style[name]) return {
                        end: transEndEventNames[name]
                    };
                return !1
            }
            $.fn.emulateTransitionEnd = function(duration) {
                var called = !1,
                    $el = this;
                $(this).one("bsTransitionEnd", function() {
                    called = !0
                });
                var callback = function() {
                    called || $($el).trigger($.support.transition.end)
                };
                return setTimeout(callback, duration), this
            }, $(function() {
                $.support.transition = transitionEnd(), $.support.transition && ($.event.special.bsTransitionEnd = {
                    bindType: $.support.transition.end,
                    delegateType: $.support.transition.end,
                    handle: function(e) {
                        return $(e.target).is(this) ? e.handleObj.handler.apply(this, arguments) : void 0
                    }
                })
            })
        }(jQuery), + function($) {
            "use strict";

            function Plugin(option) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("bs.alert");
                    data || $this.data("bs.alert", data = new Alert(this)), "string" == typeof option && data[option].call($this)
                })
            }
            var dismiss = '[data-dismiss="alert"]',
                Alert = function(el) {
                    $(el).on("click", dismiss, this.close)
                };
            Alert.VERSION = "3.3.1", Alert.TRANSITION_DURATION = 150, Alert.prototype.close = function(e) {
                function removeElement() {
                    $parent.detach().trigger("closed.bs.alert").remove()
                }
                var $this = $(this),
                    selector = $this.attr("data-target");
                selector || (selector = $this.attr("href"), selector = selector && selector.replace(/.*(?=#[^\s]*$)/, ""));
                var $parent = $(selector);
                e && e.preventDefault(), $parent.length || ($parent = $this.closest(".alert")), $parent.trigger(e = $.Event("close.bs.alert")), e.isDefaultPrevented() || ($parent.removeClass("in"), $.support.transition && $parent.hasClass("fade") ? $parent.one("bsTransitionEnd", removeElement).emulateTransitionEnd(Alert.TRANSITION_DURATION) : removeElement())
            };
            var old = $.fn.alert;
            $.fn.alert = Plugin, $.fn.alert.Constructor = Alert, $.fn.alert.noConflict = function() {
                return $.fn.alert = old, this
            }, $(document).on("click.bs.alert.data-api", dismiss, Alert.prototype.close)
        }(jQuery), + function($) {
            "use strict";

            function Plugin(option) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("bs.button"),
                        options = "object" == typeof option && option;
                    data || $this.data("bs.button", data = new Button(this, options)), "toggle" == option ? data.toggle() : option && data.setState(option)
                })
            }
            var Button = function(element, options) {
                this.$element = $(element), this.options = $.extend({}, Button.DEFAULTS, options), this.isLoading = !1
            };
            Button.VERSION = "3.3.1", Button.DEFAULTS = {
                loadingText: "loading..."
            }, Button.prototype.setState = function(state) {
                var d = "disabled",
                    $el = this.$element,
                    val = $el.is("input") ? "val" : "html",
                    data = $el.data();
                state += "Text", null == data.resetText && $el.data("resetText", $el[val]()), setTimeout($.proxy(function() {
                    $el[val](null == data[state] ? this.options[state] : data[state]), "loadingText" == state ? (this.isLoading = !0, $el.addClass(d).attr(d, d)) : this.isLoading && (this.isLoading = !1, $el.removeClass(d).removeAttr(d))
                }, this), 0)
            }, Button.prototype.toggle = function() {
                var changed = !0,
                    $parent = this.$element.closest('[data-toggle="buttons"]');
                if ($parent.length) {
                    var $input = this.$element.find("input");
                    "radio" == $input.prop("type") && ($input.prop("checked") && this.$element.hasClass("active") ? changed = !1 : $parent.find(".active").removeClass("active")), changed && $input.prop("checked", !this.$element.hasClass("active")).trigger("change")
                } else this.$element.attr("aria-pressed", !this.$element.hasClass("active"));
                changed && this.$element.toggleClass("active")
            };
            var old = $.fn.button;
            $.fn.button = Plugin, $.fn.button.Constructor = Button, $.fn.button.noConflict = function() {
                return $.fn.button = old, this
            }, $(document).on("click.bs.button.data-api", '[data-toggle^="button"]', function(e) {
                var $btn = $(e.target);
                $btn.hasClass("btn") || ($btn = $btn.closest(".btn")), Plugin.call($btn, "toggle"), e.preventDefault()
            }).on("focus.bs.button.data-api blur.bs.button.data-api", '[data-toggle^="button"]', function(e) {
                $(e.target).closest(".btn").toggleClass("focus", /^focus(in)?$/.test(e.type))
            })
        }(jQuery), + function($) {
            "use strict";

            function getTargetFromTrigger($trigger) {
                var href, target = $trigger.attr("data-target") || (href = $trigger.attr("href")) && href.replace(/.*(?=#[^\s]+$)/, "");
                return $(target)
            }

            function Plugin(option) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("bs.collapse"),
                        options = $.extend({}, Collapse.DEFAULTS, $this.data(), "object" == typeof option && option);
                    !data && options.toggle && "show" == option && (options.toggle = !1), data || $this.data("bs.collapse", data = new Collapse(this, options)), "string" == typeof option && data[option]()
                })
            }
            var Collapse = function(element, options) {
                this.$element = $(element), this.options = $.extend({}, Collapse.DEFAULTS, options), this.$trigger = $(this.options.trigger).filter('[href="#' + element.id + '"], [data-target="#' + element.id + '"]'), this.transitioning = null, this.options.parent ? this.$parent = this.getParent() : this.addAriaAndCollapsedClass(this.$element, this.$trigger), this.options.toggle && this.toggle()
            };
            Collapse.VERSION = "3.3.1", Collapse.TRANSITION_DURATION = 350, Collapse.DEFAULTS = {
                toggle: !0,
                trigger: '[data-toggle="collapse"]'
            }, Collapse.prototype.dimension = function() {
                var hasWidth = this.$element.hasClass("width");
                return hasWidth ? "width" : "height"
            }, Collapse.prototype.show = function() {
                if (!this.transitioning && !this.$element.hasClass("in")) {
                    var activesData, actives = this.$parent && this.$parent.find("> .panel").children(".in, .collapsing");
                    if (!(actives && actives.length && (activesData = actives.data("bs.collapse"), activesData && activesData.transitioning))) {
                        var startEvent = $.Event("show.bs.collapse");
                        if (this.$element.trigger(startEvent), !startEvent.isDefaultPrevented()) {
                            actives && actives.length && (Plugin.call(actives, "hide"), activesData || actives.data("bs.collapse", null));
                            var dimension = this.dimension();
                            this.$element.removeClass("collapse").addClass("collapsing")[dimension](0).attr("aria-expanded", !0), this.$trigger.removeClass("collapsed").attr("aria-expanded", !0), this.transitioning = 1;
                            var complete = function() {
                                this.$element.removeClass("collapsing").addClass("collapse in")[dimension](""), this.transitioning = 0, this.$element.trigger("shown.bs.collapse")
                            };
                            if (!$.support.transition) return complete.call(this);
                            var scrollSize = $.camelCase(["scroll", dimension].join("-"));
                            this.$element.one("bsTransitionEnd", $.proxy(complete, this)).emulateTransitionEnd(Collapse.TRANSITION_DURATION)[dimension](this.$element[0][scrollSize])
                        }
                    }
                }
            }, Collapse.prototype.hide = function() {
                if (!this.transitioning && this.$element.hasClass("in")) {
                    var startEvent = $.Event("hide.bs.collapse");
                    if (this.$element.trigger(startEvent), !startEvent.isDefaultPrevented()) {
                        var dimension = this.dimension();
                        this.$element[dimension](this.$element[dimension]())[0].offsetHeight, this.$element.addClass("collapsing").removeClass("collapse in").attr("aria-expanded", !1), this.$trigger.addClass("collapsed").attr("aria-expanded", !1), this.transitioning = 1;
                        var complete = function() {
                            this.transitioning = 0, this.$element.removeClass("collapsing").addClass("collapse").trigger("hidden.bs.collapse")
                        };
                        return $.support.transition ? void this.$element[dimension](0).one("bsTransitionEnd", $.proxy(complete, this)).emulateTransitionEnd(Collapse.TRANSITION_DURATION) : complete.call(this)
                    }
                }
            }, Collapse.prototype.toggle = function() {
                this[this.$element.hasClass("in") ? "hide" : "show"]()
            }, Collapse.prototype.getParent = function() {
                return $(this.options.parent).find('[data-toggle="collapse"][data-parent="' + this.options.parent + '"]').each($.proxy(function(i, element) {
                    var $element = $(element);
                    this.addAriaAndCollapsedClass(getTargetFromTrigger($element), $element)
                }, this)).end()
            }, Collapse.prototype.addAriaAndCollapsedClass = function($element, $trigger) {
                var isOpen = $element.hasClass("in");
                $element.attr("aria-expanded", isOpen), $trigger.toggleClass("collapsed", !isOpen).attr("aria-expanded", isOpen)
            };
            var old = $.fn.collapse;
            $.fn.collapse = Plugin, $.fn.collapse.Constructor = Collapse, $.fn.collapse.noConflict = function() {
                return $.fn.collapse = old, this
            }, $(document).on("click.bs.collapse.data-api", '[data-toggle="collapse"]', function(e) {
                var $this = $(this);
                $this.attr("data-target") || e.preventDefault();
                var $target = getTargetFromTrigger($this),
                    data = $target.data("bs.collapse"),
                    option = data ? "toggle" : $.extend({}, $this.data(), {
                        trigger: this
                    });
                Plugin.call($target, option)
            })
        }(jQuery), + function($) {
            "use strict";

            function clearMenus(e) {
                e && 3 === e.which || ($(backdrop).remove(), $(toggle).each(function() {
                    var $this = $(this),
                        $parent = getParent($this),
                        relatedTarget = {
                            relatedTarget: this
                        };
                    $parent.hasClass("open") && ($parent.trigger(e = $.Event("hide.bs.dropdown", relatedTarget)), e.isDefaultPrevented() || ($this.attr("aria-expanded", "false"), $parent.removeClass("open").trigger("hidden.bs.dropdown", relatedTarget)))
                }))
            }

            function getParent($this) {
                var selector = $this.attr("data-target");
                selector || (selector = $this.attr("href"), selector = selector && /#[A-Za-z]/.test(selector) && selector.replace(/.*(?=#[^\s]*$)/, ""));
                var $parent = selector && $(selector);
                return $parent && $parent.length ? $parent : $this.parent()
            }

            function Plugin(option) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("bs.dropdown");
                    data || $this.data("bs.dropdown", data = new Dropdown(this)), "string" == typeof option && data[option].call($this)
                })
            }
            var backdrop = ".dropdown-backdrop",
                toggle = '[data-toggle="dropdown"]',
                Dropdown = function(element) {
                    $(element).on("click.bs.dropdown", this.toggle)
                };
            Dropdown.VERSION = "3.3.1", Dropdown.prototype.toggle = function(e) {
                var $this = $(this);
                if (!$this.is(".disabled, :disabled")) {
                    var $parent = getParent($this),
                        isActive = $parent.hasClass("open");
                    if (clearMenus(), !isActive) {
                        "ontouchstart" in document.documentElement && !$parent.closest(".navbar-nav").length && $('<div class="dropdown-backdrop"/>').insertAfter($(this)).on("click", clearMenus);
                        var relatedTarget = {
                            relatedTarget: this
                        };
                        if ($parent.trigger(e = $.Event("show.bs.dropdown", relatedTarget)), e.isDefaultPrevented()) return;
                        $this.trigger("focus").attr("aria-expanded", "true"), $parent.toggleClass("open").trigger("shown.bs.dropdown", relatedTarget)
                    }
                    return !1
                }
            }, Dropdown.prototype.keydown = function(e) {
                if (/(38|40|27|32)/.test(e.which) && !/input|textarea/i.test(e.target.tagName)) {
                    var $this = $(this);
                    if (e.preventDefault(), e.stopPropagation(), !$this.is(".disabled, :disabled")) {
                        var $parent = getParent($this),
                            isActive = $parent.hasClass("open");
                        if (!isActive && 27 != e.which || isActive && 27 == e.which) return 27 == e.which && $parent.find(toggle).trigger("focus"), $this.trigger("click");
                        var desc = " li:not(.divider):visible a",
                            $items = $parent.find('[role="menu"]' + desc + ', [role="listbox"]' + desc);
                        if ($items.length) {
                            var index = $items.index(e.target);
                            38 == e.which && index > 0 && index--, 40 == e.which && index < $items.length - 1 && index++, ~index || (index = 0), $items.eq(index).trigger("focus")
                        }
                    }
                }
            };
            var old = $.fn.dropdown;
            $.fn.dropdown = Plugin, $.fn.dropdown.Constructor = Dropdown, $.fn.dropdown.noConflict = function() {
                return $.fn.dropdown = old, this
            }, $(document).on("click.bs.dropdown.data-api", clearMenus).on("click.bs.dropdown.data-api", ".dropdown form", function(e) {
                e.stopPropagation()
            }).on("click.bs.dropdown.data-api", toggle, Dropdown.prototype.toggle).on("keydown.bs.dropdown.data-api", toggle, Dropdown.prototype.keydown).on("keydown.bs.dropdown.data-api", '[role="menu"]', Dropdown.prototype.keydown).on("keydown.bs.dropdown.data-api", '[role="listbox"]', Dropdown.prototype.keydown)
        }(jQuery), + function($) {
            "use strict";

            function Plugin(option, _relatedTarget) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("bs.modal"),
                        options = $.extend({}, Modal.DEFAULTS, $this.data(), "object" == typeof option && option);
                    data || $this.data("bs.modal", data = new Modal(this, options)), "string" == typeof option ? data[option](_relatedTarget) : options.show && data.show(_relatedTarget)
                })
            }
            var Modal = function(element, options) {
                this.options = options, this.$body = $(document.body), this.$element = $(element), this.$backdrop = this.isShown = null, this.scrollbarWidth = 0, this.options.remote && this.$element.find(".modal-content").load(this.options.remote, $.proxy(function() {
                    this.$element.trigger("loaded.bs.modal")
                }, this))
            };
            Modal.VERSION = "3.3.1", Modal.TRANSITION_DURATION = 300, Modal.BACKDROP_TRANSITION_DURATION = 150, Modal.DEFAULTS = {
                backdrop: !0,
                keyboard: !0,
                show: !0
            }, Modal.prototype.toggle = function(_relatedTarget) {
                return this.isShown ? this.hide() : this.show(_relatedTarget)
            }, Modal.prototype.show = function(_relatedTarget) {
                var that = this,
                    e = $.Event("show.bs.modal", {
                        relatedTarget: _relatedTarget
                    });
                this.$element.trigger(e), this.isShown || e.isDefaultPrevented() || (this.isShown = !0, this.checkScrollbar(), this.setScrollbar(), this.$body.addClass("modal-open"), this.escape(), this.resize(), this.$element.on("click.dismiss.bs.modal", '[data-dismiss="modal"]', $.proxy(this.hide, this)), this.backdrop(function() {
                    var transition = $.support.transition && that.$element.hasClass("fade");
                    that.$element.parent().length || that.$element.appendTo(that.$body), that.$element.show().scrollTop(0), that.options.backdrop && that.adjustBackdrop(), that.adjustDialog(), transition && that.$element[0].offsetWidth, that.$element.addClass("in").attr("aria-hidden", !1), that.enforceFocus();
                    var e = $.Event("shown.bs.modal", {
                        relatedTarget: _relatedTarget
                    });
                    transition ? that.$element.find(".modal-dialog").one("bsTransitionEnd", function() {
                        that.$element.trigger("focus").trigger(e)
                    }).emulateTransitionEnd(Modal.TRANSITION_DURATION) : that.$element.trigger("focus").trigger(e)
                }))
            }, Modal.prototype.hide = function(e) {
                e && e.preventDefault(), e = $.Event("hide.bs.modal"), this.$element.trigger(e), this.isShown && !e.isDefaultPrevented() && (this.isShown = !1, this.escape(), this.resize(), $(document).off("focusin.bs.modal"), this.$element.removeClass("in").attr("aria-hidden", !0).off("click.dismiss.bs.modal"), $.support.transition && this.$element.hasClass("fade") ? this.$element.one("bsTransitionEnd", $.proxy(this.hideModal, this)).emulateTransitionEnd(Modal.TRANSITION_DURATION) : this.hideModal())
            }, Modal.prototype.enforceFocus = function() {
                $(document).off("focusin.bs.modal").on("focusin.bs.modal", $.proxy(function(e) {
                    this.$element[0] === e.target || this.$element.has(e.target).length || this.$element.trigger("focus")
                }, this))
            }, Modal.prototype.escape = function() {
                this.isShown && this.options.keyboard ? this.$element.on("keydown.dismiss.bs.modal", $.proxy(function(e) {
                    27 == e.which && this.hide()
                }, this)) : this.isShown || this.$element.off("keydown.dismiss.bs.modal")
            }, Modal.prototype.resize = function() {
                this.isShown ? $(window).on("resize.bs.modal", $.proxy(this.handleUpdate, this)) : $(window).off("resize.bs.modal")
            }, Modal.prototype.hideModal = function() {
                var that = this;
                this.$element.hide(), this.backdrop(function() {
                    that.$body.removeClass("modal-open"), that.resetAdjustments(), that.resetScrollbar(), that.$element.trigger("hidden.bs.modal")
                })
            }, Modal.prototype.removeBackdrop = function() {
                this.$backdrop && this.$backdrop.remove(), this.$backdrop = null
            }, Modal.prototype.backdrop = function(callback) {
                var that = this,
                    animate = this.$element.hasClass("fade") ? "fade" : "";
                if (this.isShown && this.options.backdrop) {
                    var doAnimate = $.support.transition && animate;
                    if (this.$backdrop = $('<div class="modal-backdrop ' + animate + '" />').prependTo(this.$element).on("click.dismiss.bs.modal", $.proxy(function(e) {
                            e.target === e.currentTarget && ("static" == this.options.backdrop ? this.$element[0].focus.call(this.$element[0]) : this.hide.call(this))
                        }, this)), doAnimate && this.$backdrop[0].offsetWidth, this.$backdrop.addClass("in"), !callback) return;
                    doAnimate ? this.$backdrop.one("bsTransitionEnd", callback).emulateTransitionEnd(Modal.BACKDROP_TRANSITION_DURATION) : callback()
                } else if (!this.isShown && this.$backdrop) {
                    this.$backdrop.removeClass("in");
                    var callbackRemove = function() {
                        that.removeBackdrop(), callback && callback()
                    };
                    $.support.transition && this.$element.hasClass("fade") ? this.$backdrop.one("bsTransitionEnd", callbackRemove).emulateTransitionEnd(Modal.BACKDROP_TRANSITION_DURATION) : callbackRemove()
                } else callback && callback()
            }, Modal.prototype.handleUpdate = function() {
                this.options.backdrop && this.adjustBackdrop(), this.adjustDialog()
            }, Modal.prototype.adjustBackdrop = function() {
                this.$backdrop.css("height", 0).css("height", this.$element[0].scrollHeight)
            }, Modal.prototype.adjustDialog = function() {
                var modalIsOverflowing = this.$element[0].scrollHeight > document.documentElement.clientHeight;
                this.$element.css({
                    paddingLeft: !this.bodyIsOverflowing && modalIsOverflowing ? this.scrollbarWidth : "",
                    paddingRight: this.bodyIsOverflowing && !modalIsOverflowing ? this.scrollbarWidth : ""
                })
            }, Modal.prototype.resetAdjustments = function() {
                this.$element.css({
                    paddingLeft: "",
                    paddingRight: ""
                })
            }, Modal.prototype.checkScrollbar = function() {
                this.bodyIsOverflowing = document.body.scrollHeight > document.documentElement.clientHeight, this.scrollbarWidth = this.measureScrollbar()
            }, Modal.prototype.setScrollbar = function() {
                var bodyPad = parseInt(this.$body.css("padding-right") || 0, 10);
                this.bodyIsOverflowing && this.$body.css("padding-right", bodyPad + this.scrollbarWidth)
            }, Modal.prototype.resetScrollbar = function() {
                this.$body.css("padding-right", "")
            }, Modal.prototype.measureScrollbar = function() {
                var scrollDiv = document.createElement("div");
                scrollDiv.className = "modal-scrollbar-measure", this.$body.append(scrollDiv);
                var scrollbarWidth = scrollDiv.offsetWidth - scrollDiv.clientWidth;
                return this.$body[0].removeChild(scrollDiv), scrollbarWidth
            };
            var old = $.fn.modal;
            $.fn.modal = Plugin, $.fn.modal.Constructor = Modal, $.fn.modal.noConflict = function() {
                return $.fn.modal = old, this
            }, $(document).on("click.bs.modal.data-api", '[data-toggle="modal"]', function(e) {
                var $this = $(this),
                    href = $this.attr("href"),
                    $target = $($this.attr("data-target") || href && href.replace(/.*(?=#[^\s]+$)/, "")),
                    option = $target.data("bs.modal") ? "toggle" : $.extend({
                        remote: !/#/.test(href) && href
                    }, $target.data(), $this.data());
                $this.is("a") && e.preventDefault(), $target.one("show.bs.modal", function(showEvent) {
                    showEvent.isDefaultPrevented() || $target.one("hidden.bs.modal", function() {
                        $this.is(":visible") && $this.trigger("focus")
                    })
                }), Plugin.call($target, option, this)
            })
        }(jQuery), + function($) {
            "use strict";

            function Plugin(option) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("bs.tooltip"),
                        options = "object" == typeof option && option,
                        selector = options && options.selector;
                    (data || "destroy" != option) && (selector ? (data || $this.data("bs.tooltip", data = {}), data[selector] || (data[selector] = new Tooltip(this, options))) : data || $this.data("bs.tooltip", data = new Tooltip(this, options)), "string" == typeof option && data[option]())
                })
            }
            var Tooltip = function(element, options) {
                this.type = this.options = this.enabled = this.timeout = this.hoverState = this.$element = null, this.init("tooltip", element, options)
            };
            Tooltip.VERSION = "3.3.1", Tooltip.TRANSITION_DURATION = 150, Tooltip.DEFAULTS = {
                animation: !0,
                placement: "top",
                selector: !1,
                template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
                trigger: "hover focus",
                title: "",
                delay: 0,
                html: !1,
                container: !1,
                viewport: {
                    selector: "body",
                    padding: 0
                }
            }, Tooltip.prototype.init = function(type, element, options) {
                this.enabled = !0, this.type = type, this.$element = $(element), this.options = this.getOptions(options), this.$viewport = this.options.viewport && $(this.options.viewport.selector || this.options.viewport);
                for (var triggers = this.options.trigger.split(" "), i = triggers.length; i--;) {
                    var trigger = triggers[i];
                    if ("click" == trigger) this.$element.on("click." + this.type, this.options.selector, $.proxy(this.toggle, this));
                    else if ("manual" != trigger) {
                        var eventIn = "hover" == trigger ? "mouseenter" : "focusin",
                            eventOut = "hover" == trigger ? "mouseleave" : "focusout";
                        this.$element.on(eventIn + "." + this.type, this.options.selector, $.proxy(this.enter, this)), this.$element.on(eventOut + "." + this.type, this.options.selector, $.proxy(this.leave, this))
                    }
                }
                this.options.selector ? this._options = $.extend({}, this.options, {
                    trigger: "manual",
                    selector: ""
                }) : this.fixTitle()
            }, Tooltip.prototype.getDefaults = function() {
                return Tooltip.DEFAULTS
            }, Tooltip.prototype.getOptions = function(options) {
                return options = $.extend({}, this.getDefaults(), this.$element.data(), options), options.delay && "number" == typeof options.delay && (options.delay = {
                    show: options.delay,
                    hide: options.delay
                }), options
            }, Tooltip.prototype.getDelegateOptions = function() {
                var options = {},
                    defaults = this.getDefaults();
                return this._options && $.each(this._options, function(key, value) {
                    defaults[key] != value && (options[key] = value)
                }), options
            }, Tooltip.prototype.enter = function(obj) {
                var self = obj instanceof this.constructor ? obj : $(obj.currentTarget).data("bs." + this.type);
                return self && self.$tip && self.$tip.is(":visible") ? void(self.hoverState = "in") : (self || (self = new this.constructor(obj.currentTarget, this.getDelegateOptions()), $(obj.currentTarget).data("bs." + this.type, self)), clearTimeout(self.timeout), self.hoverState = "in", self.options.delay && self.options.delay.show ? void(self.timeout = setTimeout(function() {
                    "in" == self.hoverState && self.show()
                }, self.options.delay.show)) : self.show())
            }, Tooltip.prototype.leave = function(obj) {
                var self = obj instanceof this.constructor ? obj : $(obj.currentTarget).data("bs." + this.type);
                return self || (self = new this.constructor(obj.currentTarget, this.getDelegateOptions()), $(obj.currentTarget).data("bs." + this.type, self)), clearTimeout(self.timeout), self.hoverState = "out", self.options.delay && self.options.delay.hide ? void(self.timeout = setTimeout(function() {
                    "out" == self.hoverState && self.hide()
                }, self.options.delay.hide)) : self.hide()
            }, Tooltip.prototype.show = function() {
                var e = $.Event("show.bs." + this.type);
                if (this.hasContent() && this.enabled) {
                    this.$element.trigger(e);
                    var inDom = $.contains(this.$element[0].ownerDocument.documentElement, this.$element[0]);
                    if (e.isDefaultPrevented() || !inDom) return;
                    var that = this,
                        $tip = this.tip(),
                        tipId = this.getUID(this.type);
                    this.setContent(), $tip.attr("id", tipId), this.$element.attr("aria-describedby", tipId), this.options.animation && $tip.addClass("fade");
                    var placement = "function" == typeof this.options.placement ? this.options.placement.call(this, $tip[0], this.$element[0]) : this.options.placement,
                        autoToken = /\s?auto?\s?/i,
                        autoPlace = autoToken.test(placement);
                    autoPlace && (placement = placement.replace(autoToken, "") || "top"), $tip.detach().css({
                        top: 0,
                        left: 0,
                        display: "block"
                    }).addClass(placement).data("bs." + this.type, this), this.options.container ? $tip.appendTo(this.options.container) : $tip.insertAfter(this.$element);
                    var pos = this.getPosition(),
                        actualWidth = $tip[0].offsetWidth,
                        actualHeight = $tip[0].offsetHeight;
                    if (autoPlace) {
                        var orgPlacement = placement,
                            $container = this.options.container ? $(this.options.container) : this.$element.parent(),
                            containerDim = this.getPosition($container);
                        placement = "bottom" == placement && pos.bottom + actualHeight > containerDim.bottom ? "top" : "top" == placement && pos.top - actualHeight < containerDim.top ? "bottom" : "right" == placement && pos.right + actualWidth > containerDim.width ? "left" : "left" == placement && pos.left - actualWidth < containerDim.left ? "right" : placement, $tip.removeClass(orgPlacement).addClass(placement)
                    }
                    var calculatedOffset = this.getCalculatedOffset(placement, pos, actualWidth, actualHeight);
                    this.applyPlacement(calculatedOffset, placement);
                    var complete = function() {
                        var prevHoverState = that.hoverState;
                        that.$element.trigger("shown.bs." + that.type), that.hoverState = null, "out" == prevHoverState && that.leave(that)
                    };
                    $.support.transition && this.$tip.hasClass("fade") ? $tip.one("bsTransitionEnd", complete).emulateTransitionEnd(Tooltip.TRANSITION_DURATION) : complete()
                }
            }, Tooltip.prototype.applyPlacement = function(offset, placement) {
                var $tip = this.tip(),
                    width = $tip[0].offsetWidth,
                    height = $tip[0].offsetHeight,
                    marginTop = parseInt($tip.css("margin-top"), 10),
                    marginLeft = parseInt($tip.css("margin-left"), 10);
                isNaN(marginTop) && (marginTop = 0), isNaN(marginLeft) && (marginLeft = 0), offset.top = offset.top + marginTop, offset.left = offset.left + marginLeft, $.offset.setOffset($tip[0], $.extend({
                    using: function(props) {
                        $tip.css({
                            top: Math.round(props.top),
                            left: Math.round(props.left)
                        })
                    }
                }, offset), 0), $tip.addClass("in");
                var actualWidth = $tip[0].offsetWidth,
                    actualHeight = $tip[0].offsetHeight;
                "top" == placement && actualHeight != height && (offset.top = offset.top + height - actualHeight);
                var delta = this.getViewportAdjustedDelta(placement, offset, actualWidth, actualHeight);
                delta.left ? offset.left += delta.left : offset.top += delta.top;
                var isVertical = /top|bottom/.test(placement),
                    arrowDelta = isVertical ? 2 * delta.left - width + actualWidth : 2 * delta.top - height + actualHeight,
                    arrowOffsetPosition = isVertical ? "offsetWidth" : "offsetHeight";
                $tip.offset(offset), this.replaceArrow(arrowDelta, $tip[0][arrowOffsetPosition], isVertical)
            }, Tooltip.prototype.replaceArrow = function(delta, dimension, isHorizontal) {
                this.arrow().css(isHorizontal ? "left" : "top", 50 * (1 - delta / dimension) + "%").css(isHorizontal ? "top" : "left", "")
            }, Tooltip.prototype.setContent = function() {
                var $tip = this.tip(),
                    title = this.getTitle();
                $tip.find(".tooltip-inner")[this.options.html ? "html" : "text"](title), $tip.removeClass("fade in top bottom left right")
            }, Tooltip.prototype.hide = function(callback) {
                function complete() {
                    "in" != that.hoverState && $tip.detach(), that.$element.removeAttr("aria-describedby").trigger("hidden.bs." + that.type), callback && callback()
                }
                var that = this,
                    $tip = this.tip(),
                    e = $.Event("hide.bs." + this.type);
                return this.$element.trigger(e), e.isDefaultPrevented() ? void 0 : ($tip.removeClass("in"), $.support.transition && this.$tip.hasClass("fade") ? $tip.one("bsTransitionEnd", complete).emulateTransitionEnd(Tooltip.TRANSITION_DURATION) : complete(), this.hoverState = null, this)
            }, Tooltip.prototype.fixTitle = function() {
                var $e = this.$element;
                ($e.attr("title") || "string" != typeof $e.attr("data-original-title")) && $e.attr("data-original-title", $e.attr("title") || "").attr("title", "")
            }, Tooltip.prototype.hasContent = function() {
                return this.getTitle()
            }, Tooltip.prototype.getPosition = function($element) {
                $element = $element || this.$element;
                var el = $element[0],
                    isBody = "BODY" == el.tagName,
                    elRect = el.getBoundingClientRect();
                null == elRect.width && (elRect = $.extend({}, elRect, {
                    width: elRect.right - elRect.left,
                    height: elRect.bottom - elRect.top
                }));
                var elOffset = isBody ? {
                        top: 0,
                        left: 0
                    } : $element.offset(),
                    scroll = {
                        scroll: isBody ? document.documentElement.scrollTop || document.body.scrollTop : $element.scrollTop()
                    },
                    outerDims = isBody ? {
                        width: $(window).width(),
                        height: $(window).height()
                    } : null;
                return $.extend({}, elRect, scroll, outerDims, elOffset)
            }, Tooltip.prototype.getCalculatedOffset = function(placement, pos, actualWidth, actualHeight) {
                return "bottom" == placement ? {
                    top: pos.top + pos.height,
                    left: pos.left + pos.width / 2 - actualWidth / 2
                } : "top" == placement ? {
                    top: pos.top - actualHeight,
                    left: pos.left + pos.width / 2 - actualWidth / 2
                } : "left" == placement ? {
                    top: pos.top + pos.height / 2 - actualHeight / 2,
                    left: pos.left - actualWidth
                } : {
                    top: pos.top + pos.height / 2 - actualHeight / 2,
                    left: pos.left + pos.width
                }
            }, Tooltip.prototype.getViewportAdjustedDelta = function(placement, pos, actualWidth, actualHeight) {
                var delta = {
                    top: 0,
                    left: 0
                };
                if (!this.$viewport) return delta;
                var viewportPadding = this.options.viewport && this.options.viewport.padding || 0,
                    viewportDimensions = this.getPosition(this.$viewport);
                if (/right|left/.test(placement)) {
                    var topEdgeOffset = pos.top - viewportPadding - viewportDimensions.scroll,
                        bottomEdgeOffset = pos.top + viewportPadding - viewportDimensions.scroll + actualHeight;
                    topEdgeOffset < viewportDimensions.top ? delta.top = viewportDimensions.top - topEdgeOffset : bottomEdgeOffset > viewportDimensions.top + viewportDimensions.height && (delta.top = viewportDimensions.top + viewportDimensions.height - bottomEdgeOffset)
                } else {
                    var leftEdgeOffset = pos.left - viewportPadding,
                        rightEdgeOffset = pos.left + viewportPadding + actualWidth;
                    leftEdgeOffset < viewportDimensions.left ? delta.left = viewportDimensions.left - leftEdgeOffset : rightEdgeOffset > viewportDimensions.width && (delta.left = viewportDimensions.left + viewportDimensions.width - rightEdgeOffset)
                }
                return delta
            }, Tooltip.prototype.getTitle = function() {
                var title, $e = this.$element,
                    o = this.options;
                return title = $e.attr("data-original-title") || ("function" == typeof o.title ? o.title.call($e[0]) : o.title)
            }, Tooltip.prototype.getUID = function(prefix) {
                do prefix += ~~(1e6 * Math.random()); while (document.getElementById(prefix));
                return prefix
            }, Tooltip.prototype.tip = function() {
                return this.$tip = this.$tip || $(this.options.template)
            }, Tooltip.prototype.arrow = function() {
                return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow")
            }, Tooltip.prototype.enable = function() {
                this.enabled = !0
            }, Tooltip.prototype.disable = function() {
                this.enabled = !1
            }, Tooltip.prototype.toggleEnabled = function() {
                this.enabled = !this.enabled
            }, Tooltip.prototype.toggle = function(e) {
                var self = this;
                e && (self = $(e.currentTarget).data("bs." + this.type), self || (self = new this.constructor(e.currentTarget, this.getDelegateOptions()), $(e.currentTarget).data("bs." + this.type, self))), self.tip().hasClass("in") ? self.leave(self) : self.enter(self)
            }, Tooltip.prototype.destroy = function() {
                var that = this;
                clearTimeout(this.timeout), this.hide(function() {
                    that.$element.off("." + that.type).removeData("bs." + that.type)
                })
            };
            var old = $.fn.tooltip;
            $.fn.tooltip = Plugin, $.fn.tooltip.Constructor = Tooltip, $.fn.tooltip.noConflict = function() {
                return $.fn.tooltip = old, this
            }
        }(jQuery), + function($) {
            "use strict";

            function Plugin(option) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("bs.popover"),
                        options = "object" == typeof option && option,
                        selector = options && options.selector;
                    (data || "destroy" != option) && (selector ? (data || $this.data("bs.popover", data = {}), data[selector] || (data[selector] = new Popover(this, options))) : data || $this.data("bs.popover", data = new Popover(this, options)), "string" == typeof option && data[option]())
                })
            }
            var Popover = function(element, options) {
                this.init("popover", element, options)
            };
            if (!$.fn.tooltip) throw new Error("Popover requires tooltip.js");
            Popover.VERSION = "3.3.1", Popover.DEFAULTS = $.extend({}, $.fn.tooltip.Constructor.DEFAULTS, {
                placement: "right",
                trigger: "click",
                content: "",
                template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
            }), Popover.prototype = $.extend({}, $.fn.tooltip.Constructor.prototype), Popover.prototype.constructor = Popover, Popover.prototype.getDefaults = function() {
                return Popover.DEFAULTS
            }, Popover.prototype.setContent = function() {
                var $tip = this.tip(),
                    title = this.getTitle(),
                    content = this.getContent();
                $tip.find(".popover-title")[this.options.html ? "html" : "text"](title), $tip.find(".popover-content").children().detach().end()[this.options.html ? "string" == typeof content ? "html" : "append" : "text"](content), $tip.removeClass("fade top bottom left right in"), $tip.find(".popover-title").html() || $tip.find(".popover-title").hide()
            }, Popover.prototype.hasContent = function() {
                return this.getTitle() || this.getContent()
            }, Popover.prototype.getContent = function() {
                var $e = this.$element,
                    o = this.options;
                return $e.attr("data-content") || ("function" == typeof o.content ? o.content.call($e[0]) : o.content)
            }, Popover.prototype.arrow = function() {
                return this.$arrow = this.$arrow || this.tip().find(".arrow")
            }, Popover.prototype.tip = function() {
                return this.$tip || (this.$tip = $(this.options.template)), this.$tip
            };
            var old = $.fn.popover;
            $.fn.popover = Plugin, $.fn.popover.Constructor = Popover, $.fn.popover.noConflict = function() {
                return $.fn.popover = old, this
            }
        }(jQuery), + function($) {
            "use strict";

            function Plugin(option) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("bs.tab");
                    data || $this.data("bs.tab", data = new Tab(this)), "string" == typeof option && data[option]()
                })
            }
            var Tab = function(element) {
                this.element = $(element)
            };
            Tab.VERSION = "3.3.1", Tab.TRANSITION_DURATION = 150, Tab.prototype.show = function() {
                var $this = this.element,
                    $ul = $this.closest("ul:not(.dropdown-menu)"),
                    selector = $this.data("target");
                if (selector || (selector = $this.attr("href"), selector = selector && selector.replace(/.*(?=#[^\s]*$)/, "")), !$this.parent("li").hasClass("active")) {
                    var $previous = $ul.find(".active:last a"),
                        hideEvent = $.Event("hide.bs.tab", {
                            relatedTarget: $this[0]
                        }),
                        showEvent = $.Event("show.bs.tab", {
                            relatedTarget: $previous[0]
                        });
                    if ($previous.trigger(hideEvent), $this.trigger(showEvent), !showEvent.isDefaultPrevented() && !hideEvent.isDefaultPrevented()) {
                        var $target = $(selector);
                        this.activate($this.closest("li"), $ul), this.activate($target, $target.parent(), function() {
                            $previous.trigger({
                                type: "hidden.bs.tab",
                                relatedTarget: $this[0]
                            }), $this.trigger({
                                type: "shown.bs.tab",
                                relatedTarget: $previous[0]
                            })
                        })
                    }
                }
            }, Tab.prototype.activate = function(element, container, callback) {
                function next() {
                    $active.removeClass("active").find("> .dropdown-menu > .active").removeClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !1), element.addClass("active").find('[data-toggle="tab"]').attr("aria-expanded", !0), transition ? (element[0].offsetWidth, element.addClass("in")) : element.removeClass("fade"), element.parent(".dropdown-menu") && element.closest("li.dropdown").addClass("active").end().find('[data-toggle="tab"]').attr("aria-expanded", !0), callback && callback()
                }
                var $active = container.find("> .active"),
                    transition = callback && $.support.transition && ($active.length && $active.hasClass("fade") || !!container.find("> .fade").length);
                $active.length && transition ? $active.one("bsTransitionEnd", next).emulateTransitionEnd(Tab.TRANSITION_DURATION) : next(), $active.removeClass("in")
            };
            var old = $.fn.tab;
            $.fn.tab = Plugin, $.fn.tab.Constructor = Tab, $.fn.tab.noConflict = function() {
                return $.fn.tab = old, this
            };
            var clickHandler = function(e) {
                e.preventDefault(), Plugin.call($(this), "show")
            };
            $(document).on("click.bs.tab.data-api", '[data-toggle="tab"]', clickHandler).on("click.bs.tab.data-api", '[data-toggle="pill"]', clickHandler)
        }(jQuery),
        function(window, factory) {
            var lazySizes = factory(window, window.document);
            window.lazySizes = lazySizes, "object" == typeof module && module.exports ? module.exports = lazySizes : "function" == typeof define && define.amd && define(lazySizes)
        }(window, function(window, document) {
            "use strict";
            if (document.getElementsByClassName) {
                var lazySizesConfig, docElem = document.documentElement,
                    addEventListener = window.addEventListener,
                    setTimeout = window.setTimeout,
                    rAF = window.requestAnimationFrame || setTimeout,
                    setImmediate = window.setImmediate || setTimeout,
                    regPicture = /^picture$/i,
                    loadEvents = ["load", "error", "lazyincluded", "_lazyloaded"],
                    hasClass = function(ele, cls) {
                        var reg = new RegExp("(\\s|^)" + cls + "(\\s|$)");
                        return ele.className.match(reg) && reg
                    },
                    addClass = function(ele, cls) {
                        hasClass(ele, cls) || (ele.className += " " + cls)
                    },
                    removeClass = function(ele, cls) {
                        var reg;
                        (reg = hasClass(ele, cls)) && (ele.className = ele.className.replace(reg, " "))
                    },
                    addRemoveLoadEvents = function(dom, fn, add) {
                        var action = add ? "addEventListener" : "removeEventListener";
                        add && addRemoveLoadEvents(dom, fn), loadEvents.forEach(function(evt) {
                            dom[action](evt, fn)
                        })
                    },
                    triggerEvent = function(elem, name, detail, noBubbles, noCancelable) {
                        var event = document.createEvent("CustomEvent");
                        return event.initCustomEvent(name, !noBubbles, !noCancelable, detail || {}), elem.dispatchEvent(event), event
                    },
                    updatePolyfill = function(el, full) {
                        var polyfill;
                        window.HTMLPictureElement || ((polyfill = window.picturefill || window.respimage || lazySizesConfig.pf) ? polyfill({
                            reevaluate: !0,
                            elements: [el]
                        }) : full && full.src && (el.src = full.src))
                    },
                    getCSS = function(elem, style) {
                        return (getComputedStyle(elem, null) || {})[style]
                    },
                    getWidth = function(elem, parent, width) {
                        for (width = width || elem.offsetWidth; width < lazySizesConfig.minSize && parent && !elem._lazysizesWidth;) width = parent.offsetWidth, parent = parent.parentNode;
                        return width
                    },
                    throttle = function(fn) {
                        var running, lastTime = 0,
                            Date = window.Date,
                            run = function() {
                                running = !1, lastTime = Date.now(), fn()
                            },
                            afterAF = function() {
                                setImmediate(run)
                            },
                            getAF = function() {
                                rAF(afterAF)
                            };
                        return function() {
                            if (!running) {
                                var delay = lazySizesConfig.throttle - (Date.now() - lastTime);
                                running = !0, 6 > delay && (delay = 6), setTimeout(getAF, delay)
                            }
                        }
                    },
                    loader = function() {
                        var lazyloadElems, preloadElems, isCompleted, resetPreloadingTimer, loadMode, eLvW, elvH, eLtop, eLleft, eLright, eLbottom, defaultExpand, preloadExpand, regImg = /^img$/i,
                            regIframe = /^iframe$/i,
                            supportScroll = "onscroll" in window && !/glebot/.test(navigator.userAgent),
                            shrinkExpand = 0,
                            currentExpand = 0,
                            isLoading = 0,
                            lowRuns = 0,
                            resetPreloading = function(e) {
                                isLoading--, e && e.target && addRemoveLoadEvents(e.target, resetPreloading), (!e || 0 > isLoading || !e.target) && (isLoading = 0)
                            },
                            isNestedVisible = function(elem, elemExpand) {
                                var outerRect, parent = elem,
                                    visible = "hidden" != getCSS(elem, "visibility");
                                for (eLtop -= elemExpand, eLbottom += elemExpand, eLleft -= elemExpand, eLright += elemExpand; visible && (parent = parent.offsetParent);) visible = (getCSS(parent, "opacity") || 1) > 0, visible && "visible" != getCSS(parent, "overflow") && (outerRect = parent.getBoundingClientRect(), visible = eLright > outerRect.left && eLleft < outerRect.right && eLbottom > outerRect.top - 1 && eLtop < outerRect.bottom + 1);
                                return visible
                            },
                            checkElements = function() {
                                var eLlen, i, rect, autoLoadElem, loadedSomething, elemExpand, elemNegativeExpand, elemExpandVal, beforeExpandVal;
                                if ((loadMode = lazySizesConfig.loadMode) && 8 > isLoading && (eLlen = lazyloadElems.length)) {
                                    for (i = 0, lowRuns++, preloadExpand > currentExpand && 1 > isLoading && lowRuns > 3 && loadMode > 2 ? (currentExpand = preloadExpand, lowRuns = 0) : currentExpand = currentExpand != defaultExpand && loadMode > 1 && lowRuns > 2 && 6 > isLoading ? defaultExpand : shrinkExpand; eLlen > i; i++)
                                        if (lazyloadElems[i] && !lazyloadElems[i]._lazyRace)
                                            if (supportScroll)
                                                if ((elemExpandVal = lazyloadElems[i].getAttribute("data-expand")) && (elemExpand = 1 * elemExpandVal) || (elemExpand = currentExpand), beforeExpandVal !== elemExpand && (eLvW = innerWidth + elemExpand, elvH = innerHeight + elemExpand, elemNegativeExpand = -1 * elemExpand, beforeExpandVal = elemExpand), rect = lazyloadElems[i].getBoundingClientRect(), (eLbottom = rect.bottom) >= elemNegativeExpand && (eLtop = rect.top) <= elvH && (eLright = rect.right) >= elemNegativeExpand && (eLleft = rect.left) <= eLvW && (eLbottom || eLright || eLleft || eLtop) && (isCompleted && 3 > isLoading && !elemExpandVal && (3 > loadMode || 4 > lowRuns) || isNestedVisible(lazyloadElems[i], elemExpand))) {
                                                    if (unveilElement(lazyloadElems[i], rect.width), loadedSomething = !0, isLoading > 9) break
                                                } else !loadedSomething && isCompleted && !autoLoadElem && 3 > isLoading && 4 > lowRuns && loadMode > 2 && (preloadElems[0] || lazySizesConfig.preloadAfterLoad) && (preloadElems[0] || !elemExpandVal && (eLbottom || eLright || eLleft || eLtop || "auto" != lazyloadElems[i].getAttribute(lazySizesConfig.sizesAttr))) && (autoLoadElem = preloadElems[0] || lazyloadElems[i]);
                                    else unveilElement(lazyloadElems[i]);
                                    autoLoadElem && !loadedSomething && unveilElement(autoLoadElem)
                                }
                            },
                            throttledCheckElements = throttle(checkElements),
                            switchLoadingClass = function(e) {
                                addClass(e.target, lazySizesConfig.loadedClass), removeClass(e.target, lazySizesConfig.loadingClass), addRemoveLoadEvents(e.target, switchLoadingClass)
                            },
                            changeIframeSrc = function(elem, src) {
                                try {
                                    elem.contentWindow.location.replace(src)
                                } catch (e) {
                                    elem.setAttribute("src", src)
                                }
                            },
                            rafBatch = function() {
                                var isRunning, batch = [],
                                    runBatch = function() {
                                        for (; batch.length;) batch.shift()();
                                        isRunning = !1
                                    };
                                return function(fn) {
                                    batch.push(fn), isRunning || (isRunning = !0, rAF(runBatch))
                                }
                            }(),
                            unveilElement = function(elem, width) {
                                var sources, i, len, sourceSrcset, src, srcset, parent, isPicture, event, firesLoad, customMedia, isImg = regImg.test(elem.nodeName),
                                    sizes = isImg && (elem.getAttribute(lazySizesConfig.sizesAttr) || elem.getAttribute("sizes")),
                                    isAuto = "auto" == sizes;
                                (!isAuto && isCompleted || !isImg || !elem.src && !elem.srcset || elem.complete || hasClass(elem, lazySizesConfig.errorClass)) && (elem._lazyRace = !0, isLoading++, rafBatch(function() {
                                    if (elem._lazyRace && delete elem._lazyRace, removeClass(elem, lazySizesConfig.lazyClass), !(event = triggerEvent(elem, "lazybeforeunveil")).defaultPrevented) {
                                        if (sizes && (isAuto ? (autoSizer.updateElem(elem, !0, width), addClass(elem, lazySizesConfig.autosizesClass)) : elem.setAttribute("sizes", sizes)), srcset = elem.getAttribute(lazySizesConfig.srcsetAttr), src = elem.getAttribute(lazySizesConfig.srcAttr), isImg && (parent = elem.parentNode, isPicture = parent && regPicture.test(parent.nodeName || "")), firesLoad = event.detail.firesLoad || "src" in elem && (srcset || src || isPicture), event = {
                                                target: elem
                                            }, firesLoad && (addRemoveLoadEvents(elem, resetPreloading, !0), clearTimeout(resetPreloadingTimer), resetPreloadingTimer = setTimeout(resetPreloading, 2500), addClass(elem, lazySizesConfig.loadingClass), addRemoveLoadEvents(elem, switchLoadingClass, !0)), isPicture)
                                            for (sources = parent.getElementsByTagName("source"), i = 0, len = sources.length; len > i; i++)(customMedia = lazySizesConfig.customMedia[sources[i].getAttribute("data-media") || sources[i].getAttribute("media")]) && sources[i].setAttribute("media", customMedia), sourceSrcset = sources[i].getAttribute(lazySizesConfig.srcsetAttr), sourceSrcset && sources[i].setAttribute("srcset", sourceSrcset);
                                        srcset ? elem.setAttribute("srcset", srcset) : src && (regIframe.test(elem.nodeName) ? changeIframeSrc(elem, src) : elem.setAttribute("src", src)), (srcset || isPicture) && updatePolyfill(elem, {
                                            src: src
                                        })
                                    }(!firesLoad || elem.complete) && (firesLoad ? resetPreloading(event) : isLoading--, switchLoadingClass(event))
                                }))
                            },
                            onload = function() {
                                var scrollTimer, afterScroll = function() {
                                    lazySizesConfig.loadMode = 3, throttledCheckElements()
                                };
                                isCompleted = !0, lowRuns += 8, lazySizesConfig.loadMode = 3, addEventListener("scroll", function() {
                                    3 == lazySizesConfig.loadMode && (lazySizesConfig.loadMode = 2), clearTimeout(scrollTimer), scrollTimer = setTimeout(afterScroll, 99)
                                }, !0)
                            };
                        return {
                            _: function() {
                                lazyloadElems = document.getElementsByClassName(lazySizesConfig.lazyClass), preloadElems = document.getElementsByClassName(lazySizesConfig.lazyClass + " " + lazySizesConfig.preloadClass), defaultExpand = lazySizesConfig.expand, preloadExpand = Math.round(defaultExpand * lazySizesConfig.expFactor), addEventListener("scroll", throttledCheckElements, !0), addEventListener("resize", throttledCheckElements, !0), window.MutationObserver ? new MutationObserver(throttledCheckElements).observe(docElem, {
                                    childList: !0,
                                    subtree: !0,
                                    attributes: !0
                                }) : (docElem.addEventListener("DOMNodeInserted", throttledCheckElements, !0), docElem.addEventListener("DOMAttrModified", throttledCheckElements, !0), setInterval(throttledCheckElements, 999)), addEventListener("hashchange", throttledCheckElements, !0), ["focus", "mouseover", "click", "load", "transitionend", "animationend", "webkitAnimationEnd"].forEach(function(name) {
                                    document.addEventListener(name, throttledCheckElements, !0)
                                }), /d$|^c/.test(document.readyState) ? onload() : (addEventListener("load", onload), document.addEventListener("DOMContentLoaded", throttledCheckElements)), throttledCheckElements()
                            },
                            checkElems: throttledCheckElements,
                            unveil: unveilElement
                        }
                    }(),
                    autoSizer = function() {
                        var autosizesElems, sizeElement = function(elem, dataAttr, width) {
                                var sources, i, len, event, parent = elem.parentNode;
                                if (parent && (width = getWidth(elem, parent, width), event = triggerEvent(elem, "lazybeforesizes", {
                                        width: width,
                                        dataAttr: !!dataAttr
                                    }), !event.defaultPrevented && (width = event.detail.width, width && width !== elem._lazysizesWidth))) {
                                    if (elem._lazysizesWidth = width, width += "px", elem.setAttribute("sizes", width), regPicture.test(parent.nodeName || ""))
                                        for (sources = parent.getElementsByTagName("source"), i = 0, len = sources.length; len > i; i++) sources[i].setAttribute("sizes", width);
                                    event.detail.dataAttr || updatePolyfill(elem, event.detail)
                                }
                            },
                            updateElementsSizes = function() {
                                var i, len = autosizesElems.length;
                                if (len)
                                    for (i = 0; len > i; i++) sizeElement(autosizesElems[i])
                            },
                            throttledUpdateElementsSizes = throttle(updateElementsSizes);
                        return {
                            _: function() {
                                autosizesElems = document.getElementsByClassName(lazySizesConfig.autosizesClass), addEventListener("resize", throttledUpdateElementsSizes)
                            },
                            checkElems: throttledUpdateElementsSizes,
                            updateElem: sizeElement
                        }
                    }(),
                    init = function() {
                        init.i || (init.i = !0, autoSizer._(), loader._())
                    };
                return function() {
                    var prop, lazySizesDefaults = {
                        lazyClass: "lazyload",
                        loadedClass: "lazyloaded",
                        loadingClass: "lazyloading",
                        preloadClass: "lazypreload",
                        errorClass: "lazyerror",
                        autosizesClass: "lazyautosizes",
                        srcAttr: "data-src",
                        srcsetAttr: "data-srcset",
                        sizesAttr: "data-sizes",
                        minSize: 40,
                        customMedia: {},
                        init: !0,
                        expFactor: 2,
                        expand: 359,
                        loadMode: 2,
                        throttle: 125
                    };
                    lazySizesConfig = window.lazySizesConfig || window.lazysizesConfig || {};
                    for (prop in lazySizesDefaults) prop in lazySizesConfig || (lazySizesConfig[prop] = lazySizesDefaults[prop]);
                    window.lazySizesConfig = lazySizesConfig, setTimeout(function() {
                        lazySizesConfig.init && init()
                    })
                }(), {
                    cfg: lazySizesConfig,
                    autoSizer: autoSizer,
                    loader: loader,
                    init: init,
                    uP: updatePolyfill,
                    aC: addClass,
                    rC: removeClass,
                    hC: hasClass,
                    fire: triggerEvent,
                    gW: getWidth
                }
            }
        }), ! function($) {
            var WinReszier = function() {
                    var timer, registered = [],
                        inited = !1,
                        resize = function() {
                            clearTimeout(timer), timer = setTimeout(notify, 100)
                        },
                        notify = function() {
                            for (var i = 0, cnt = registered.length; cnt > i; i++) registered[i].apply()
                        };
                    return {
                        register: function(fn) {
                            registered.push(fn), inited === !1 && ($(window).bind("resize", resize), inited = !0)
                        },
                        unregister: function(fn) {
                            for (var i = 0, cnt = registered.length; cnt > i; i++)
                                if (registered[i] == fn) {
                                    delete registered[i];
                                    break
                                }
                        }
                    }
                }(),
                TabDrop = function(element, options) {
                    this.element = $(element), this.dropdown = $('<li class="dropdown hide pull-right tabdrop"><a class="dropdown-toggle" data-toggle="dropdown" href="#">' + options.text + ' <b class="caret"></b></a><ul class="dropdown-menu"></ul></li>').prependTo(this.element), this.element.parent().is(".tabs-below") && this.dropdown.addClass("dropup"), WinReszier.register($.proxy(this.layout, this)), this.layout()
                };
            TabDrop.prototype = {
                constructor: TabDrop,
                layout: function() {
                    var collection = [];
                    this.dropdown.removeClass("hide"), this.element.append(this.dropdown.find("li")).find(">li").not(".tabdrop").each(function() {
                        this.offsetTop > 0 && collection.push(this)
                    }), collection.length > 0 ? (collection = $(collection), this.dropdown.find("ul").empty().append(collection), 1 == this.dropdown.find(".active").length ? this.dropdown.addClass("active") : this.dropdown.removeClass("active")) : this.dropdown.addClass("hide")
                }
            }, $.fn.tabdrop = function(option) {
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("tabdrop"),
                        options = "object" == typeof option && option;
                    data || $this.data("tabdrop", data = new TabDrop(this, $.extend({}, $.fn.tabdrop.defaults, options))), "string" == typeof option && data[option]()
                })
            }, $.fn.tabdrop.defaults = {
                text: '<i class="icon-align-justify"></i>'
            }, $.fn.tabdrop.Constructor = TabDrop
        }(window.jQuery);
    var fakewaffle = function($, fakewaffle) {
        "use strict";
        return fakewaffle.responsiveTabs = function(collapseDisplayed) {
            fakewaffle.currentPosition = "tabs";
            var tabGroups = $(".nav-tabs.responsive"),
                hidden = "",
                visible = "",
                activeTab = "";
            void 0 === collapseDisplayed && (collapseDisplayed = ["xs", "sm"]), $.each(collapseDisplayed, function() {
                hidden += " hidden-" + this, visible += " visible-" + this
            }), $.each(tabGroups, function() {
                var $tabGroup = $(this),
                    tabs = $tabGroup.find("li a"),
                    collapseDiv = $("<div></div>", {
                        "class": "panel-group responsive" + visible,
                        id: "collapse-" + $tabGroup.attr("id")
                    });
                $.each(tabs, function() {
                    var $this = $(this),
                        oldLinkClass = void 0 === $this.attr("class") ? "" : $this.attr("class"),
                        newLinkClass = "accordion-toggle",
                        oldParentClass = void 0 === $this.parent().attr("class") ? "" : $this.parent().attr("class"),
                        newParentClass = "panel panel-default",
                        newHash = $this.get(0).hash.replace("#", "collapse-");
                    oldLinkClass.length > 0 && (newLinkClass += " " + oldLinkClass), oldParentClass.length > 0 && (oldParentClass = oldParentClass.replace(/\bactive\b/g, ""), newParentClass += " " + oldParentClass, newParentClass = newParentClass.replace(/\s{2,}/g, " "), newParentClass = newParentClass.replace(/^\s+|\s+$/g, "")), $this.parent().hasClass("active") && (activeTab = "#" + newHash), collapseDiv.append($("<div>").attr("class", newParentClass).html($("<div>").attr("class", "panel-heading").html($("<h4>").attr("class", "panel-title").html($("<a>", {
                        "class": newLinkClass,
                        "data-toggle": "collapse",
                        "data-parent": "#collapse-" + $tabGroup.attr("id"),
                        href: "#" + newHash,
                        html: $this.html()
                    })))).append($("<div>", {
                        id: newHash,
                        "class": "panel-collapse collapse"
                    })))
                }), $tabGroup.next().after(collapseDiv), $tabGroup.addClass(hidden), $(".tab-content.responsive").addClass(hidden)
            }), fakewaffle.checkResize(), fakewaffle.bindTabToCollapse(), activeTab && $(activeTab).collapse("show")
        }, fakewaffle.checkResize = function() {
            $(".panel-group.responsive").is(":visible") === !0 && "tabs" === fakewaffle.currentPosition ? (fakewaffle.tabToPanel(), fakewaffle.currentPosition = "panel") : $(".panel-group.responsive").is(":visible") === !1 && "panel" === fakewaffle.currentPosition && (fakewaffle.panelToTab(), fakewaffle.currentPosition = "tabs")
        }, fakewaffle.tabToPanel = function() {
            var tabGroups = $(".nav-tabs.responsive");
            $.each(tabGroups, function(index, tabGroup) {
                var tabContents = $(tabGroup).next(".tab-content").find(".tab-pane");
                $.each(tabContents, function(index, tabContent) {
                    var destinationId = $(tabContent).attr("id").replace(/^/, "#collapse-");
                    $(tabContent).removeClass("tab-pane").addClass("panel-body").appendTo($(destinationId))
                })
            })
        }, fakewaffle.panelToTab = function() {
            var panelGroups = $(".panel-group.responsive");
            $.each(panelGroups, function(index, panelGroup) {
                var destinationId = $(panelGroup).attr("id").replace("collapse-", "#"),
                    destination = $(destinationId).next(".tab-content")[0],
                    panelContents = $(panelGroup).find(".panel-body");
                panelContents.removeClass("panel-body").addClass("tab-pane").appendTo($(destination))
            })
        }, fakewaffle.bindTabToCollapse = function() {
            var tabs = $(".nav-tabs.responsive").find("li a"),
                collapse = $(".panel-group.responsive").find(".panel-collapse");
            tabs.on("shown.bs.tab", function(e) {
                var $current = $(e.currentTarget.hash.replace(/#/, "#collapse-"));
                if ($current.collapse("show"), e.relatedTarget) {
                    var $previous = $(e.relatedTarget.hash.replace(/#/, "#collapse-"));
                    $previous.collapse("hide")
                }
            }), collapse.on("shown.bs.collapse", function(e) {
                var current = $(e.target).context.id.replace(/collapse-/g, "#");
                $('a[href="' + current + '"]').tab("show");
                var panelGroup = $(e.currentTarget).closest(".panel-group.responsive");
                $(panelGroup).find(".panel-body").removeClass("active"), $(e.currentTarget).find(".panel-body").addClass("active")
            })
        }, $(window).resize(function() {
            fakewaffle.checkResize()
        }), fakewaffle
    }(window.jQuery, fakewaffle || {});
    ! function($) {//PRINCIPIO SLIDER ???
        var Slider = function(element, options) {
            this.element = $(element), this.picker = $('<div class="slider"><div class="slider-track"><div class="slider-selection"></div><div class="slider-handle"></div><div class="slider-handle"></div></div><div class="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div></div>').insertBefore(this.element).append(this.element), this.id = this.element.data("slider-id") || options.id, this.id && (this.picker[0].id = this.id), "undefined" != typeof Modernizr && Modernizr.touch && (this.touchCapable = !0);
            var tooltip = this.element.data("slider-tooltip") || options.tooltip;
            switch (this.tooltip = this.picker.find(".tooltip"), this.tooltipInner = this.tooltip.find("div.tooltip-inner"), this.orientation = this.element.data("slider-orientation") || options.orientation, this.orientation) {
                case "vertical":
                    this.picker.addClass("slider-vertical"), this.stylePos = "top", this.mousePos = "pageY", this.sizePos = "offsetHeight", this.tooltip.addClass("right")[0].style.left = "100%";
                    break;
                default:
                    this.picker.addClass("slider-horizontal"), this.orientation = "horizontal", this.stylePos = "left", this.mousePos = "pageX", this.sizePos = "offsetWidth", this.tooltip.addClass("top")[0].style.top = -this.tooltip.outerHeight() - 14 + "px"
            }
            this.min = this.element.data("slider-min") || options.min, this.max = this.element.data("slider-max") || options.max, this.step = this.element.data("slider-step") || options.step, this.value = this.element.data("slider-value") || options.value, this.value[1] && (this.range = !0), this.selection = this.element.data("slider-selection") || options.selection, this.selectionEl = this.picker.find(".slider-selection"), "none" === this.selection && this.selectionEl.addClass("hide"), this.selectionElStyle = this.selectionEl[0].style, this.handle1 = this.picker.find(".slider-handle:first"), this.handle1Stype = this.handle1[0].style, this.handle2 = this.picker.find(".slider-handle:last"), this.handle2Stype = this.handle2[0].style;
            var handle = this.element.data("slider-handle") || options.handle;
            switch (handle) {
                case "round":
                    this.handle1.addClass("round"), this.handle2.addClass("round");
                    break;
                case "triangle":
                    this.handle1.addClass("triangle"), this.handle2.addClass("triangle")
            }
            this.range ? (this.value[0] = Math.max(this.min, Math.min(this.max, this.value[0])), this.value[1] = Math.max(this.min, Math.min(this.max, this.value[1]))) : (this.value = [Math.max(this.min, Math.min(this.max, this.value))], this.handle2.addClass("hide"), this.value[1] = "after" == this.selection ? this.max : this.min), this.diff = this.max - this.min, this.percentage = [100 * (this.value[0] - this.min) / this.diff, 100 * (this.value[1] - this.min) / this.diff, 100 * this.step / this.diff], this.offset = this.picker.offset(), this.size = this.picker[0][this.sizePos], this.formater = options.formater, this.layout(), this.picker.on(this.touchCapable ? {
                touchstart: $.proxy(this.mousedown, this)
            } : {
                mousedown: $.proxy(this.mousedown, this)
            }), "show" === tooltip ? this.picker.on({
                mouseenter: $.proxy(this.showTooltip, this),
                mouseleave: $.proxy(this.hideTooltip, this)
            }) : this.tooltip.addClass("hide")
        };
        Slider.prototype = {
            constructor: Slider,
            over: !1,
            inDrag: !1,
            showTooltip: function() {
                this.tooltip.addClass("in"), this.over = !0
            },
            hideTooltip: function() {
                this.inDrag === !1 && this.tooltip.removeClass("in"), this.over = !1
            },
            layout: function() {
                this.handle1Stype[this.stylePos] = this.percentage[0] + "%", this.handle2Stype[this.stylePos] = this.percentage[1] + "%", "vertical" == this.orientation ? (this.selectionElStyle.top = Math.min(this.percentage[0], this.percentage[1]) + "%", this.selectionElStyle.height = Math.abs(this.percentage[0] - this.percentage[1]) + "%") : (this.selectionElStyle.left = Math.min(this.percentage[0], this.percentage[1]) + "%", this.selectionElStyle.width = Math.abs(this.percentage[0] - this.percentage[1]) + "%"), this.range ? (this.tooltipInner.text(this.formater(this.value[0]) + " : " + this.formater(this.value[1])), this.tooltip[0].style[this.stylePos] = this.size * (this.percentage[0] + (this.percentage[1] - this.percentage[0]) / 2) / 100 - ("vertical" === this.orientation ? this.tooltip.outerHeight() / 2 : this.tooltip.outerWidth() / 2) + "px") : (this.tooltipInner.text(this.formater(this.value[0])), this.tooltip[0].style[this.stylePos] = this.size * this.percentage[0] / 100 - ("vertical" === this.orientation ? this.tooltip.outerHeight() / 2 : this.tooltip.outerWidth() / 2) + "px")
            },
            mousedown: function(ev) {
                this.touchCapable && "touchstart" === ev.type && (ev = ev.originalEvent), this.offset = this.picker.offset(), this.size = this.picker[0][this.sizePos];
                var percentage = this.getPercentage(ev);
                if (this.range) {
                    var diff1 = Math.abs(this.percentage[0] - percentage),
                        diff2 = Math.abs(this.percentage[1] - percentage);
                    this.dragged = diff2 > diff1 ? 0 : 1
                } else this.dragged = 0;
                this.percentage[this.dragged] = percentage, this.layout(), $(document).on(this.touchCapable ? {
                    touchmove: $.proxy(this.mousemove, this),
                    touchend: $.proxy(this.mouseup, this)
                } : {
                    mousemove: $.proxy(this.mousemove, this),
                    mouseup: $.proxy(this.mouseup, this)
                }), this.inDrag = !0;
                var val = this.calculateValue();
                return this.element.trigger({
                    type: "slideStart",
                    value: val
                }).trigger({
                    type: "slide",
                    value: val
                }), !1
            },
            mousemove: function(ev) {
                this.touchCapable && "touchmove" === ev.type && (ev = ev.originalEvent);
                var percentage = this.getPercentage(ev);
                this.range && (0 === this.dragged && this.percentage[1] < percentage ? (this.percentage[0] = this.percentage[1], this.dragged = 1) : 1 === this.dragged && this.percentage[0] > percentage && (this.percentage[1] = this.percentage[0], this.dragged = 0)), this.percentage[this.dragged] = percentage, this.layout();
                var val = this.calculateValue();
                return this.element.trigger({
                    type: "slide",
                    value: val
                }).data("value", val).prop("value", val), !1
            },
            mouseup: function() {
                $(document).off(this.touchCapable ? {
                    touchmove: this.mousemove,
                    touchend: this.mouseup
                } : {
                    mousemove: this.mousemove,
                    mouseup: this.mouseup
                }), this.inDrag = !1, 0 == this.over && this.hideTooltip(), this.element;
                var val = this.calculateValue();
                return this.element.trigger({
                    type: "slideStop",
                    value: val
                }).data("value", val).prop("value", val), !1
            },
            calculateValue: function() {
                var val;
                return this.range ? (val = [this.min + Math.round(this.diff * this.percentage[0] / 100 / this.step) * this.step, this.min + Math.round(this.diff * this.percentage[1] / 100 / this.step) * this.step], this.value = val) : (val = this.min + Math.round(this.diff * this.percentage[0] / 100 / this.step) * this.step, this.value = [val, this.value[1]]), val
            },
            getPercentage: function(ev) {
                this.touchCapable && (ev = ev.touches[0]);
                var percentage = 100 * (ev[this.mousePos] - this.offset[this.stylePos]) / this.size;
                return percentage = Math.round(percentage / this.percentage[2]) * this.percentage[2], Math.max(0, Math.min(100, percentage))
            },
            getValue: function() {
                return this.range ? this.value : this.value[0]
            },
            setValue: function(val) {
                this.value = val, this.range ? (this.value[0] = Math.max(this.min, Math.min(this.max, this.value[0])), this.value[1] = Math.max(this.min, Math.min(this.max, this.value[1]))) : (this.value = [Math.max(this.min, Math.min(this.max, this.value))], this.handle2.addClass("hide"), this.value[1] = "after" == this.selection ? this.max : this.min), this.diff = this.max - this.min, this.percentage = [100 * (this.value[0] - this.min) / this.diff, 100 * (this.value[1] - this.min) / this.diff, 100 * this.step / this.diff], this.layout()
            }
        }, $.fn.slider = function(option, val) {
            return this.each(function() {
                var $this = $(this),
                    data = $this.data("slider"),
                    options = "object" == typeof option && option;
                data || $this.data("slider", data = new Slider(this, $.extend({}, $.fn.slider.defaults, options))), "string" == typeof option && data[option](val)
            })
        }, $.fn.slider.defaults = {
            min: 0,
            max: 10,
            step: 1,
            orientation: "horizontal",
            value: 5,
            selection: "before",
            tooltip: "show",
            handle: "round",
            formater: function(value) {
                return value
            }
        }, $.fn.slider.Constructor = Slider
    }(window.jQuery),
    function($) { //PRINCIPIO COMPONENTE BOOTSTRAP-SELECT
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
            return $.each(rExps, function() {
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
            Object.keys = Object.keys || function(o, k, r) {
                r = [];
                for (k in o) r.hasOwnProperty.call(o, k) && r.push(k);
                return r
            };
            var source = "(?:" + Object.keys(escapeMap).join("|") + ")",
                testRegexp = new RegExp(source),
                replaceRegexp = new RegExp(source, "g"),
                string = null == html ? "" : "" + html;
            return testRegexp.test(string) ? string.replace(replaceRegexp, function(match) {
                return escapeMap[match]
            }) : string
        }

        function Plugin(option, event) {
            var args = arguments,
                _option = option,
                option = args[0],
                event = args[1];
            [].shift.apply(args), "undefined" == typeof option && (option = _option);
            var value, chain = this.each(function() {
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
        $.expr[":"].icontains = function(obj, index, meta) {
            return icontains($(obj).text(), meta[3])
        }, $.expr[":"].aicontains = function(obj, index, meta) {
            return icontains($(obj).data("normalizedText") || $(obj).text(), meta[3])
        };
        var Selectpicker = function(element, options, e) {
            e && (e.stopPropagation(), e.preventDefault()), this.$element = $(element), this.$newElement = null, this.$button = null, this.$menu = null, this.$lis = null, this.options = options, null === this.options.title && (this.options.title = this.$element.attr("title")), this.val = Selectpicker.prototype.val, this.render = Selectpicker.prototype.render, this.refresh = Selectpicker.prototype.refresh, this.setStyle = Selectpicker.prototype.setStyle, this.selectAll = Selectpicker.prototype.selectAll, this.deselectAll = Selectpicker.prototype.deselectAll, this.destroy = Selectpicker.prototype.remove, this.remove = Selectpicker.prototype.remove, this.show = Selectpicker.prototype.show, this.hide = Selectpicker.prototype.hide, this.init()
        };
        Selectpicker.VERSION = "1.6.3", Selectpicker.DEFAULTS = {
            noneSelectedText: "Nothing selected",
            noneResultsText: "No results match",
            countSelectedText: function(numSelected) {
                return 1 == numSelected ? "{0} item selected" : "{0} items selected"
            },
            maxOptionsText: function(numAll, numGroup) {
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
            init: function() {
                var that = this,
                    id = this.$element.attr("id");
                this.$element.hide(), this.multiple = this.$element.prop("multiple"), this.autofocus = this.$element.prop("autofocus"), this.$newElement = this.createView(), this.$element.after(this.$newElement), this.$menu = this.$newElement.find("> .dropdown-menu"), this.$button = this.$newElement.find("> button"), this.$searchbox = this.$newElement.find("input"), this.options.dropdownAlignRight && this.$menu.addClass("dropdown-menu-right"), "undefined" != typeof id && (this.$button.attr("data-id", id), $('label[for="' + id + '"]').click(function(e) {
                    e.preventDefault(), that.$button.focus()
                })), this.checkDisabled(), this.clickListener(), this.options.liveSearch && this.liveSearchListener(), this.render(), this.liHeight(), this.setStyle(), this.setWidth(), this.options.container && this.selectPosition(), this.$menu.data("this", this), this.$newElement.data("this", this), this.options.mobile && this.mobile()
            },
            createDropdown: function() {
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
            createView: function() {
                var $drop = this.createDropdown(),
                    $li = this.createLi();
                return $drop.find("ul").append($li), $drop
            },
            reloadLi: function() {
                this.destroyLi();
                var $li = this.createLi();
                this.$menu.find("ul").append($li)
            },
            destroyLi: function() {
                this.$menu.find("li").remove()
            },
            createLi: function() {
                var that = this,
                    _li = [],
                    optID = 0,
                    generateLI = function(content, index, classes) {
                        return "<li" + ("undefined" != typeof classes ? ' class="' + classes + '"' : "") + ("undefined" != typeof index | null === index ? ' data-original-index="' + index + '"' : "") + ">" + content + "</li>"
                    },
                    generateA = function(text, classes, inline, optgroup) {
                        var normText = normalizeToBase(htmlEscape(text));
                        return '<a tabindex="0"' + ("undefined" != typeof classes ? ' class="' + classes + '"' : "") + ("undefined" != typeof inline ? ' style="' + inline + '"' : "") + ("undefined" != typeof optgroup ? 'data-optgroup="' + optgroup + '"' : "") + ' data-normalized-text="' + normText + '">' + text + '<span class="' + that.options.iconBase + " " + that.options.tickIcon + ' check-mark"></span></a>'
                    };
                return this.$element.find("option").each(function() {
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
            findLis: function() {
                return null == this.$lis && (this.$lis = this.$menu.find("li")), this.$lis
            },
            render: function(updateLi) {
                var that = this;
                updateLi !== !1 && this.$element.find("option").each(function(index) {
                    that.setDisabled(index, $(this).is(":disabled") || $(this).parent().is(":disabled")), that.setSelected(index, $(this).is(":selected"))
                }), this.tabIndex();
                var notDisabled = this.options.hideDisabled ? ":not([disabled])" : "",
                    selectedItems = this.$element.find("option:selected" + notDisabled).map(function() {
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
            setStyle: function(style, status) {
                this.$element.attr("class") && this.$newElement.addClass(this.$element.attr("class").replace(/selectpicker|mobile-device|validate\[.*\]/gi, ""));
                var buttonClass = style ? style : this.options.style;
                "add" == status ? this.$button.addClass(buttonClass) : "remove" == status ? this.$button.removeClass(buttonClass) : (this.$button.removeClass(this.options.style), this.$button.addClass(buttonClass))
            },
            liHeight: function() {
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
            setSize: function() {
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
                    posVert = function() {
                        selectOffsetTop = that.$newElement.offset().top - $window.scrollTop(), selectOffsetBot = $window.height() - selectOffsetTop - selectHeight
                    };
                if (posVert(), this.options.header && menu.css("padding-top", 0), "auto" == this.options.size) {
                    var getSize = function() {
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
            setWidth: function() {
                if ("auto" == this.options.width) {
                    this.$menu.css("min-width", "0");
                    var selectClone = this.$newElement.clone().appendTo("body"),
                        ulWidth = selectClone.find("> .dropdown-menu").css("width"),
                        btnWidth = selectClone.css("width", "auto").find("> button").css("width");
                    selectClone.remove(), this.$newElement.css("width", Math.max(parseInt(ulWidth), parseInt(btnWidth)) + "px")
                } else "fit" == this.options.width ? (this.$menu.css("min-width", ""), this.$newElement.css("width", "").addClass("fit-width")) : this.options.width ? (this.$menu.css("min-width", ""), this.$newElement.css("width", this.options.width)) : (this.$menu.css("min-width", ""), this.$newElement.css("width", ""));
                this.$newElement.hasClass("fit-width") && "fit" !== this.options.width && this.$newElement.removeClass("fit-width")
            },
            selectPosition: function() {
                var pos, actualHeight, that = this,
                    drop = "<div />",
                    $drop = $(drop),
                    getPlacement = function($element) {
                        $drop.addClass($element.attr("class").replace(/form-control/gi, "")).toggleClass("dropup", $element.hasClass("dropup")), pos = $element.offset(), actualHeight = $element.hasClass("dropup") ? 0 : $element[0].offsetHeight, $drop.css({
                            top: pos.top + actualHeight,
                            left: pos.left,
                            width: $element[0].offsetWidth,
                            position: "absolute"
                        })
                    };
                this.$newElement.on("click", function() {
                    that.isDisabled() || (getPlacement($(this)), $drop.appendTo(that.options.container), $drop.toggleClass("open", !$(this).hasClass("open")), $drop.append(that.$menu))
                }), $(window).resize(function() {
                    getPlacement(that.$newElement)
                }), $(window).on("scroll", function() {
                    getPlacement(that.$newElement)
                }), $("html").on("click", function(e) {
                    $(e.target).closest(that.$newElement).length < 1 && $drop.removeClass("open")
                })
            },
            setSelected: function(index, selected) {
                this.findLis(), this.$lis.filter('[data-original-index="' + index + '"]').toggleClass("selected", selected)
            },
            setDisabled: function(index, disabled) {
                this.findLis(), disabled ? this.$lis.filter('[data-original-index="' + index + '"]').addClass("disabled").find("a").attr("href", "#").attr("tabindex", -1) : this.$lis.filter('[data-original-index="' + index + '"]').removeClass("disabled").find("a").removeAttr("href").attr("tabindex", 0)
            },
            isDisabled: function() {
                return this.$element.is(":disabled")
            },
            checkDisabled: function() {
                var that = this;
                this.isDisabled() ? this.$button.addClass("disabled").attr("tabindex", -1) : (this.$button.hasClass("disabled") && this.$button.removeClass("disabled"), -1 == this.$button.attr("tabindex") && (this.$element.data("tabindex") || this.$button.removeAttr("tabindex"))), this.$button.click(function() {
                    return !that.isDisabled()
                })
            },
            tabIndex: function() {
                this.$element.is("[tabindex]") && (this.$element.data("tabindex", this.$element.attr("tabindex")), this.$button.attr("tabindex", this.$element.data("tabindex")))
            },
            clickListener: function() {
                var that = this;
                this.$newElement.on("touchstart.dropdown", ".dropdown-menu", function(e) {
                    e.stopPropagation()
                }), this.$newElement.on("click", function() {
                    that.setSize(), that.options.liveSearch || that.multiple || setTimeout(function() {
                        that.$menu.find(".selected a").focus()
                    }, 10)
                }), this.$menu.on("click", "li a", function(e) {
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
                                    maxOptionsArr[2] && (maxTxt = maxTxt.replace("{var}", maxOptionsArr[2][maxOptions > 1 ? 0 : 1]), maxTxtGrp = maxTxtGrp.replace("{var}", maxOptionsArr[2][maxOptionsGrp > 1 ? 0 : 1])), $option.prop("selected", !1), that.$menu.append($notify), maxOptions && maxReached && ($notify.append($("<div>" + maxTxt + "</div>")), that.$element.trigger("maxReached.bs.select")), maxOptionsGrp && maxReachedGrp && ($notify.append($("<div>" + maxTxtGrp + "</div>")), that.$element.trigger("maxReachedGrp.bs.select")), setTimeout(function() {
                                        that.setSelected(clickedIndex, !1)
                                    }, 10), $notify.delay(750).fadeOut(300, function() {
                                        $(this).remove()
                                    })
                                }
                            }
                        } else $options.prop("selected", !1), $option.prop("selected", !0), that.$menu.find(".selected").removeClass("selected"), that.setSelected(clickedIndex, !0);
                        that.multiple ? that.options.liveSearch && that.$searchbox.focus() : that.$button.focus(), (prevValue != that.$element.val() && that.multiple || prevIndex != that.$element.prop("selectedIndex") && !that.multiple) && that.$element.change()
                    }
                }), this.$menu.on("click", "li.disabled a, .popover-title, .popover-title :not(.close)", function(e) {
                    e.target == this && (e.preventDefault(), e.stopPropagation(), that.options.liveSearch ? that.$searchbox.focus() : that.$button.focus())
                }), this.$menu.on("click", "li.divider, li.dropdown-header", function(e) {
                    e.preventDefault(), e.stopPropagation(), that.options.liveSearch ? that.$searchbox.focus() : that.$button.focus()
                }), this.$menu.on("click", ".popover-title .close", function() {
                    that.$button.focus()
                }), this.$searchbox.on("click", function(e) {
                    e.stopPropagation()
                }), this.$menu.on("click", ".actions-btn", function(e) {
                    that.options.liveSearch ? that.$searchbox.focus() : that.$button.focus(), e.preventDefault(), e.stopPropagation(), $(this).is(".bs-select-all") ? that.selectAll() : that.deselectAll(), that.$element.change()
                }), this.$element.change(function() {
                    that.render(!1)
                })
            },
            liveSearchListener: function() {
                var that = this,
                    no_results = $('<li class="no-results"></li>');
                this.$newElement.on("click.dropdown.data-api touchstart.dropdown.data-api", function() {
                    that.$menu.find(".active").removeClass("active"), that.$searchbox.val() && (that.$searchbox.val(""), that.$lis.not(".is-hidden").removeClass("hide"), no_results.parent().length && no_results.remove()), that.multiple || that.$menu.find(".selected").addClass("active"), setTimeout(function() {
                        that.$searchbox.focus()
                    }, 10)
                }), this.$searchbox.on("click.dropdown.data-api focus.dropdown.data-api touchend.dropdown.data-api", function(e) {
                    e.stopPropagation()
                }), this.$searchbox.on("input propertychange", function() {
                    that.$searchbox.val() ? (that.options.searchAccentInsensitive ? that.$lis.not(".is-hidden").removeClass("hide").find("a").not(":aicontains(" + normalizeToBase(that.$searchbox.val()) + ")").parent().addClass("hide") : that.$lis.not(".is-hidden").removeClass("hide").find("a").not(":icontains(" + that.$searchbox.val() + ")").parent().addClass("hide"), that.$menu.find("li").filter(":visible:not(.no-results)").length ? no_results.parent().length && no_results.remove() : (no_results.parent().length && no_results.remove(), no_results.html(that.options.noneResultsText + ' "' + htmlEscape(that.$searchbox.val()) + '"').show(), that.$menu.find("li").last().after(no_results))) : (that.$lis.not(".is-hidden").removeClass("hide"), no_results.parent().length && no_results.remove()), that.$menu.find("li.active").removeClass("active"), that.$menu.find("li").filter(":visible:not(.divider)").eq(0).addClass("active").find("a").focus(), $(this).focus()
                })
            },
            val: function(value) {
                return "undefined" != typeof value ? (this.$element.val(value), this.render(), this.$element) : this.$element.val()
            },
            selectAll: function() {
                this.findLis(), this.$lis.not(".divider").not(".disabled").not(".selected").filter(":visible").find("a").click()
            },
            deselectAll: function() {
                this.findLis(), this.$lis.not(".divider").not(".disabled").filter(".selected").filter(":visible").find("a").click()
            },
            keydown: function(e) {
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
                    if (/(38|40)/.test(e.keyCode.toString(10))) index = $items.index($items.filter(":focus")), first = $items.parent(":not(.disabled):visible").first().index(), last = $items.parent(":not(.disabled):visible").last().index(), next = $items.eq(index).parent().nextAll(":not(.disabled):visible").eq(0).index(), prev = $items.eq(index).parent().prevAll(":not(.disabled):visible").eq(0).index(), nextPrev = $items.eq(next).parent().prevAll(":not(.disabled):visible").eq(0).index(), that.options.liveSearch && ($items.each(function(i) {
                        $(this).is(":not(.disabled)") && $(this).data("index", i)
                    }), index = $items.index($items.filter(".active")), first = $items.filter(":not(.disabled):visible").first().data("index"), last = $items.filter(":not(.disabled):visible").last().data("index"), next = $items.eq(index).nextAll(":not(.disabled):visible").eq(0).data("index"), prev = $items.eq(index).prevAll(":not(.disabled):visible").eq(0).data("index"), nextPrev = $items.eq(next).prevAll(":not(.disabled):visible").eq(0).data("index")), prevIndex = $this.data("prevIndex"), 38 == e.keyCode && (that.options.liveSearch && (index -= 1), index != nextPrev && index > prev && (index = prev), first > index && (index = first), index == prevIndex && (index = last)), 40 == e.keyCode && (that.options.liveSearch && (index += 1), -1 == index && (index = 0), index != nextPrev && next > index && (index = next), index > last && (index = last), index == prevIndex && (index = first)), $this.data("prevIndex", index), that.options.liveSearch ? (e.preventDefault(), $this.is(".dropdown-toggle") || ($items.removeClass("active"), $items.eq(index).addClass("active").find("a").focus(), $this.focus())) : $items.eq(index).focus();
                    else if (!$this.is("input")) {
                        var count, prevKey, keyIndex = [];
                        $items.each(function() {
                            $(this).parent().is(":not(.disabled)") && $.trim($(this).text().toLowerCase()).substring(0, 1) == keyCodeMap[e.keyCode] && keyIndex.push($(this).parent().index())
                        }), count = $(document).data("keycount"), count++, $(document).data("keycount", count), prevKey = $.trim($(":focus").text().toLowerCase()).substring(0, 1), prevKey != keyCodeMap[e.keyCode] ? (count = 1, $(document).data("keycount", count)) : count >= keyIndex.length && ($(document).data("keycount", 0), count > keyIndex.length && (count = 1)), $items.eq(keyIndex[count - 1]).focus()
                    }(/(13|32)/.test(e.keyCode.toString(10)) || /(^9$)/.test(e.keyCode.toString(10)) && that.options.selectOnTab) && isActive && (/(32)/.test(e.keyCode.toString(10)) || e.preventDefault(), that.options.liveSearch ? /(32)/.test(e.keyCode.toString(10)) || (that.$menu.find(".active a").click(), $this.focus()) : $(":focus").click(), $(document).data("keycount", 0)), (/(^9$|27)/.test(e.keyCode.toString(10)) && isActive && (that.multiple || that.options.liveSearch) || /(27)/.test(e.keyCode.toString(10)) && !isActive) && (that.$menu.parent().removeClass("open"), that.$button.focus())
                }
            },
            mobile: function() {
                this.$element.addClass("mobile-device").appendTo(this.$newElement), this.options.container && this.$menu.hide()
            },
            refresh: function() {
                this.$lis = null, this.reloadLi(), this.render(), this.setWidth(), this.setStyle(), this.checkDisabled(), this.liHeight()
            },
            update: function() {
                this.reloadLi(), this.setWidth(), this.setStyle(), this.checkDisabled(), this.liHeight()
            },
            hide: function() {
                this.$newElement.hide()
            },
            show: function() {
                this.$newElement.show()
            },
            remove: function() {
                this.$newElement.remove(), this.$element.remove()
            }
        };
        var old = $.fn.selectpicker;
        $.fn.selectpicker = Plugin, $.fn.selectpicker.Constructor = Selectpicker, $.fn.selectpicker.noConflict = function() {
            return $.fn.selectpicker = old, this
        }, $(document).data("keycount", 0).on("keydown", ".bootstrap-select [data-toggle=dropdown], .bootstrap-select [role=menu], .bs-searchbox input", Selectpicker.prototype.keydown).on("focusin.modal", ".bootstrap-select [data-toggle=dropdown], .bootstrap-select [role=menu], .bs-searchbox input", function(e) {
            e.stopPropagation()
        }), $(window).on("load.bs.select.data-api", function() {
            $(".selectpicker").each(function() {
                var $selectpicker = $(this);
                Plugin.call($selectpicker, $selectpicker.data())
            })
        })
    }(jQuery),//CREO QUE ES EL FINAL DEL COMPONENTE BOOTSTRAP-SELECT
    function($) {
        function regexFromString(inputstring) {
            return new RegExp("^" + inputstring + "$")
        }

        function executeFunctionByName(functionName, context) {
            for (var args = Array.prototype.slice.call(arguments).splice(2), namespaces = functionName.split("."), func = namespaces.pop(), i = 0; i < namespaces.length; i++) context = context[namespaces[i]];
            return context[func].apply(this, args)
        }
        var createdElements = [],
            defaults = {
                options: {
                    prependExistingHelpBlock: !1,
                    sniffHtml: !0,
                    preventSubmit: !0,
                    submitError: !1,
                    submitSuccess: !1,
                    semanticallyStrict: !1,
                    autoAdd: {
                        helpBlocks: !0
                    },
                    filter: function() {
                        return $(this).is(":visible") || $(this).is('[type="checkbox"]')
                    }
                },
                methods: {
                    init: function(options) {
                        var settings = $.extend(!0, {}, defaults);
                        settings.options = $.extend(!0, settings.options, options);
                        var $siblingElements = this,
                            uniqueForms = $.unique($siblingElements.map(function() {
                                return $(this).parents("form")[0]
                            }).toArray());
                        return $(uniqueForms).bind("submit", function(e) {
                            var $form = $(this),
                                warningsFound = 0,
                                $inputs = $form.find("input,textarea,select").not("[type=submit],[type=image]").filter(settings.options.filter);
                            $inputs.trigger("submit.validation").trigger("validationLostFocus.validation"), $inputs.each(function(i, el) {
                                var $this = $(el),
                                    $controlGroup = $this.parents(".form-group:not(.no-validate)").first();
                                $controlGroup.hasClass("has-warning") && ($controlGroup.removeClass("has-warning").addClass("has-error"), warningsFound++)
                            }), $inputs.trigger("validationLostFocus.validation"), warningsFound ? (settings.options.preventSubmit && e.preventDefault(), $form.addClass("error"), $.isFunction(settings.options.submitError) && settings.options.submitError($form, e, $inputs.jqBootstrapValidation("collectErrors", !0))) : ($form.removeClass("error"), $.isFunction(settings.options.submitSuccess) && settings.options.submitSuccess($form, e))
                        }), this.each(function() {
                            var $this = $(this),
                                $controlGroup = $this.parents(".form-group:not(.no-validate)").first(),
                                $helpBlock = $controlGroup.find(".help-block").first(),
                                $form = $this.parents("form").first(),
                                validatorNames = [];
                            if (!$helpBlock.length && settings.options.autoAdd && settings.options.autoAdd.helpBlocks && ($helpBlock = $('<div class="help-block" />'), $controlGroup.find(".controls").append($helpBlock), createdElements.push($helpBlock[0])), settings.options.sniffHtml) {
                                var message = "";
                                if (void 0 !== $this.attr("pattern") && (message = "Not in the expected format<!-- data-validation-pattern-message to override -->", $this.data("validationPatternMessage") && (message = $this.data("validationPatternMessage")), $this.data("validationPatternMessage", message), $this.data("validationPatternRegex", $this.attr("pattern"))), void 0 !== $this.attr("max") || void 0 !== $this.attr("aria-valuemax")) {
                                    var max = $this.attr(void 0 !== $this.attr("max") ? "max" : "aria-valuemax");
                                    message = "Too high: Maximum of '" + max + "'<!-- data-validation-max-message to override -->", $this.data("validationMaxMessage") && (message = $this.data("validationMaxMessage")), $this.data("validationMaxMessage", message), $this.data("validationMaxMax", max)
                                }
                                if (void 0 !== $this.attr("min") || void 0 !== $this.attr("aria-valuemin")) {
                                    var min = $this.attr(void 0 !== $this.attr("min") ? "min" : "aria-valuemin");
                                    message = "Too low: Minimum of '" + min + "'<!-- data-validation-min-message to override -->", $this.data("validationMinMessage") && (message = $this.data("validationMinMessage")), $this.data("validationMinMessage", message), $this.data("validationMinMin", min)
                                }
                                void 0 !== $this.attr("maxlength") && (message = "Too long: Maximum of '" + $this.attr("maxlength") + "' characters<!-- data-validation-maxlength-message to override -->", $this.data("validationMaxlengthMessage") && (message = $this.data("validationMaxlengthMessage")), $this.data("validationMaxlengthMessage", message), $this.data("validationMaxlengthMaxlength", $this.attr("maxlength"))), void 0 !== $this.attr("minlength") && (message = "Too short: Minimum of '" + $this.attr("minlength") + "' characters<!-- data-validation-minlength-message to override -->", $this.data("validationMinlengthMessage") && (message = $this.data("validationMinlengthMessage")), $this.data("validationMinlengthMessage", message), $this.data("validationMinlengthMinlength", $this.attr("minlength"))), (void 0 !== $this.attr("required") || void 0 !== $this.attr("aria-required")) && (message = settings.builtInValidators.required.message, $this.data("validationRequiredMessage") && (message = $this.data("validationRequiredMessage")), $this.data("validationRequiredMessage", message)), void 0 !== $this.attr("type") && "number" === $this.attr("type").toLowerCase() && (message = settings.builtInValidators.number.message, $this.data("validationNumberMessage") && (message = $this.data("validationNumberMessage")), $this.data("validationNumberMessage", message)), void 0 !== $this.attr("type") && "email" === $this.attr("type").toLowerCase() && (message = "Not a valid email address<!-- data-validator-validemail-message to override -->", $this.data("validationValidemailMessage") ? message = $this.data("validationValidemailMessage") : $this.data("validationEmailMessage") && (message = $this.data("validationEmailMessage")), $this.data("validationValidemailMessage", message)), void 0 !== $this.attr("minchecked") && (message = "Not enough options checked; Minimum of '" + $this.attr("minchecked") + "' required<!-- data-validation-minchecked-message to override -->", $this.data("validationMincheckedMessage") && (message = $this.data("validationMincheckedMessage")), $this.data("validationMincheckedMessage", message), $this.data("validationMincheckedMinchecked", $this.attr("minchecked"))), void 0 !== $this.attr("maxchecked") && (message = "Too many options checked; Maximum of '" + $this.attr("maxchecked") + "' required<!-- data-validation-maxchecked-message to override -->", $this.data("validationMaxcheckedMessage") && (message = $this.data("validationMaxcheckedMessage")), $this.data("validationMaxcheckedMessage", message), $this.data("validationMaxcheckedMaxchecked", $this.attr("maxchecked")))
                            }
                            void 0 !== $this.data("validation") && (validatorNames = $this.data("validation").split(",")), $.each($this.data(), function(i) {
                                var parts = i.replace(/([A-Z])/g, ",$1").split(",");
                                "validation" === parts[0] && parts[1] && validatorNames.push(parts[1])
                            });
                            var validatorNamesToInspect = validatorNames,
                                newValidatorNamesToInspect = [];
                            do $.each(validatorNames, function(i, el) {
                                validatorNames[i] = formatValidatorName(el)
                            }), validatorNames = $.unique(validatorNames), newValidatorNamesToInspect = [], $.each(validatorNamesToInspect, function(i, el) {
                                if (void 0 !== $this.data("validation" + el + "Shortcut")) $.each($this.data("validation" + el + "Shortcut").split(","), function(i2, el2) {
                                    newValidatorNamesToInspect.push(el2)
                                });
                                else if (settings.builtInValidators[el.toLowerCase()]) {
                                    var validator = settings.builtInValidators[el.toLowerCase()];
                                    "shortcut" === validator.type.toLowerCase() && $.each(validator.shortcut.split(","), function(i, el) {
                                        el = formatValidatorName(el), newValidatorNamesToInspect.push(el), validatorNames.push(el)
                                    })
                                }
                            }), validatorNamesToInspect = newValidatorNamesToInspect; while (validatorNamesToInspect.length > 0);
                            var validators = {};
                            $.each(validatorNames, function(i, el) {
                                var message = $this.data("validation" + el + "Message"),
                                    hasOverrideMessage = void 0 !== message,
                                    foundValidator = !1;
                                if (message = message ? message : "'" + el + "' validation failed <!-- Add attribute 'data-validation-" + el.toLowerCase() + "-message' to input to change this message -->", $.each(settings.validatorTypes, function(validatorType, validatorTemplate) {
                                        void 0 === validators[validatorType] && (validators[validatorType] = []), foundValidator || void 0 === $this.data("validation" + el + formatValidatorName(validatorTemplate.name)) || (validators[validatorType].push($.extend(!0, {
                                            name: formatValidatorName(validatorTemplate.name),
                                            message: message
                                        }, validatorTemplate.init($this, el))), foundValidator = !0)
                                    }), !foundValidator && settings.builtInValidators[el.toLowerCase()]) {
                                    var validator = $.extend(!0, {}, settings.builtInValidators[el.toLowerCase()]);
                                    hasOverrideMessage && (validator.message = message);
                                    var validatorType = validator.type.toLowerCase();
                                    "shortcut" === validatorType ? foundValidator = !0 : $.each(settings.validatorTypes, function(validatorTemplateType, validatorTemplate) {
                                        void 0 === validators[validatorTemplateType] && (validators[validatorTemplateType] = []), foundValidator || validatorType !== validatorTemplateType.toLowerCase() || ($this.data("validation" + el + formatValidatorName(validatorTemplate.name), validator[validatorTemplate.name.toLowerCase()]), validators[validatorType].push($.extend(validator, validatorTemplate.init($this, el))), foundValidator = !0)
                                    })
                                }
                                foundValidator || $.error("Cannot find validation info for '" + el + "'")
                            }), $helpBlock.data("original-contents", $helpBlock.data("original-contents") ? $helpBlock.data("original-contents") : $helpBlock.html()), $helpBlock.data("original-role", $helpBlock.data("original-role") ? $helpBlock.data("original-role") : $helpBlock.attr("role")), $controlGroup.data("original-classes", $controlGroup.data("original-clases") ? $controlGroup.data("original-classes") : $controlGroup.attr("class")), $this.data("original-aria-invalid", $this.data("original-aria-invalid") ? $this.data("original-aria-invalid") : $this.attr("aria-invalid")), $this.bind("validation.validation", function(event, params) {
                                var value = getValue($this),
                                    errorsFound = [];
                                return $.each(validators, function(validatorType, validatorTypeArray) {
                                    (value || value.length || params && params.includeEmpty || settings.validatorTypes[validatorType].blockSubmit && params && params.submitting) && $.each(validatorTypeArray, function(i, validator) {
                                        settings.validatorTypes[validatorType].validate($this, value, validator) && errorsFound.push(validator.message)
                                    })
                                }), errorsFound
                            }), $this.bind("getValidators.validation", function() {
                                return validators
                            }), $this.bind("submit.validation", function() {
                                return $this.triggerHandler("change.validation", {
                                    submitting: !0
                                })
                            }), $this.bind(["keyup", "focus", "blur", "click", "keydown", "keypress", "change"].join(".validation ") + ".validation", function(e, params) {
                                var value = getValue($this),
                                    errorsFound = [];
                                $controlGroup.find("input,textarea,select").each(function(i, el) {
                                    var oldCount = errorsFound.length;
                                    if ($.each($(el).triggerHandler("validation.validation", params), function(j, message) {
                                            errorsFound.push(message)
                                        }), errorsFound.length > oldCount) $(el).attr("aria-invalid", "true");
                                    else {
                                        var original = $this.data("original-aria-invalid");
                                        $(el).attr("aria-invalid", void 0 !== original ? original : !1)
                                    }
                                }), $form.find("input,select,textarea").not($this).not('[name="' + $this.attr("name") + '"]').trigger("validationLostFocus.validation"), errorsFound = $.unique(errorsFound.sort()), errorsFound.length ? ($controlGroup.removeClass("has-success has-error").addClass("has-warning"), $helpBlock.html(settings.options.semanticallyStrict && 1 === errorsFound.length ? errorsFound[0] + (settings.options.prependExistingHelpBlock ? $helpBlock.data("original-contents") : "") : '<ul role="alert"><li>' + errorsFound.join("</li><li>") + "</li></ul>" + (settings.options.prependExistingHelpBlock ? $helpBlock.data("original-contents") : ""))) : ($controlGroup.removeClass("has-warning has-error has-success"), value.length > 0 && $controlGroup.addClass("" /*"has-success"*/ ), $helpBlock.html($helpBlock.data("original-contents"))), "blur" === e.type && $controlGroup.removeClass("has-success")
                            }), $this.bind("validationLostFocus.validation", function() {
                                $controlGroup.removeClass("has-success")
                            })
                        })
                    },
                    destroy: function() {
                        return this.each(function() {
                            var $this = $(this),
                                $controlGroup = $this.parents(".form-group:not(.no-validate)").first(),
                                $helpBlock = $controlGroup.find(".help-block").first();
                            $this.unbind(".validation"), $helpBlock.html($helpBlock.data("original-contents")), $controlGroup.attr("class", $controlGroup.data("original-classes")), $this.attr("aria-invalid", $this.data("original-aria-invalid")), $helpBlock.attr("role", $this.data("original-role")), createdElements.indexOf($helpBlock[0]) > -1 && $helpBlock.remove()
                        })
                    },
                    collectErrors: function() {
                        var errorMessages = {};
                        return this.each(function(i, el) {
                            var $el = $(el),
                                name = $el.attr("name"),
                                errors = $el.triggerHandler("validation.validation", {
                                    includeEmpty: !0
                                });
                            errorMessages[name] = $.extend(!0, errors, errorMessages[name])
                        }), $.each(errorMessages, function(i, el) {
                            0 === el.length && delete errorMessages[i]
                        }), errorMessages
                    },
                    hasErrors: function() {
                        var errorMessages = [];
                        return this.each(function(i, el) {
                            errorMessages = errorMessages.concat($(el).triggerHandler("getValidators.validation") ? $(el).triggerHandler("validation.validation", {
                                submitting: !0
                            }) : [])
                        }), errorMessages.length > 0
                    },
                    override: function(newDefaults) {
                        defaults = $.extend(!0, defaults, newDefaults)
                    }
                },
                validatorTypes: {
                    callback: {
                        name: "callback",
                        init: function($this, name) {
                            return {
                                validatorName: name,
                                callback: $this.data("validation" + name + "Callback"),
                                lastValue: $this.val(),
                                lastValid: !0,
                                lastFinished: !0
                            }
                        },
                        validate: function($this, value, validator) {
                            if (validator.lastValue === value && validator.lastFinished) return !validator.lastValid;
                            if (validator.lastFinished === !0) {
                                validator.lastValue = value, validator.lastValid = !0, validator.lastFinished = !1;
                                var rrjqbvValidator = validator,
                                    rrjqbvThis = $this;
                                executeFunctionByName(validator.callback, window, $this, value, function(data) {
                                    rrjqbvValidator.lastValue === data.value && (rrjqbvValidator.lastValid = data.valid, data.message && (rrjqbvValidator.message = data.message), rrjqbvValidator.lastFinished = !0, rrjqbvThis.data("validation" + rrjqbvValidator.validatorName + "Message", rrjqbvValidator.message), setTimeout(function() {
                                        rrjqbvThis.trigger("change.validation")
                                    }, 1))
                                })
                            }
                            return !1
                        }
                    },
                    ajax: {
                        name: "ajax",
                        init: function($this, name) {
                            return {
                                validatorName: name,
                                url: $this.data("validation" + name + "Ajax"),
                                lastValue: $this.val(),
                                lastValid: !0,
                                lastFinished: !0
                            }
                        },
                        validate: function($this, value, validator) {
                            return "" + validator.lastValue == "" + value && validator.lastFinished === !0 ? validator.lastValid === !1 : (validator.lastFinished === !0 && (validator.lastValue = value, validator.lastValid = !0, validator.lastFinished = !1, $.ajax({
                                url: validator.url,
                                data: "value=" + value + "&field=" + $this.attr("name"),
                                dataType: "json",
                                success: function(data) {
                                    "" + validator.lastValue == "" + data.value && (validator.lastValid = !!data.valid, data.message && (validator.message = data.message), validator.lastFinished = !0, $this.data("validation" + validator.validatorName + "Message", validator.message), setTimeout(function() {
                                        $this.trigger("change.validation")
                                    }, 1))
                                },
                                failure: function() {
                                    validator.lastValid = !0, validator.message = "ajax call failed", validator.lastFinished = !0, $this.data("validation" + validator.validatorName + "Message", validator.message), setTimeout(function() {
                                        $this.trigger("change.validation")
                                    }, 1)
                                }
                            })), !1)
                        }
                    },
                    regex: {
                        name: "regex",
                        init: function($this, name) {
                            return {
                                regex: regexFromString($this.data("validation" + name + "Regex"))
                            }
                        },
                        validate: function($this, value, validator) {
                            return !validator.regex.test(value) && !validator.negative || validator.regex.test(value) && validator.negative
                        }
                    },
                    required: {
                        name: "required",
                        init: function() {
                            return {}
                        },
                        validate: function($this, value, validator) {
                            return !(0 !== value.length || validator.negative) || !!(value.length > 0 && validator.negative)
                        },
                        blockSubmit: !0
                    },
                    match: {
                        name: "match",
                        init: function($this, name) {
                            var element = $this.parents("form").first().find('[name="' + $this.data("validation" + name + "Match") + '"]').first();
                            return element.bind("validation.validation", function() {
                                $this.trigger("change.validation", {
                                    submitting: !0
                                })
                            }), {
                                element: element
                            }
                        },
                        validate: function($this, value, validator) {
                            return value !== validator.element.val() && !validator.negative || value === validator.element.val() && validator.negative
                        },
                        blockSubmit: !0
                    },
                    max: {
                        name: "max",
                        init: function($this, name) {
                            return {
                                max: $this.data("validation" + name + "Max")
                            }
                        },
                        validate: function($this, value, validator) {
                            return parseFloat(value, 10) > parseFloat(validator.max, 10) && !validator.negative || parseFloat(value, 10) <= parseFloat(validator.max, 10) && validator.negative
                        }
                    },
                    min: {
                        name: "min",
                        init: function($this, name) {
                            return {
                                min: $this.data("validation" + name + "Min")
                            }
                        },
                        validate: function($this, value, validator) {
                            return parseFloat(value) < parseFloat(validator.min) && !validator.negative || parseFloat(value) >= parseFloat(validator.min) && validator.negative
                        }
                    },
                    maxlength: {
                        name: "maxlength",
                        init: function($this, name) {
                            return {
                                maxlength: $this.data("validation" + name + "Maxlength")
                            }
                        },
                        validate: function($this, value, validator) {
                            return value.length > validator.maxlength && !validator.negative || value.length <= validator.maxlength && validator.negative
                        }
                    },
                    minlength: {
                        name: "minlength",
                        init: function($this, name) {
                            return {
                                minlength: $this.data("validation" + name + "Minlength")
                            }
                        },
                        validate: function($this, value, validator) {
                            return value.length < validator.minlength && !validator.negative || value.length >= validator.minlength && validator.negative
                        }
                    },
                    maxchecked: {
                        name: "maxchecked",
                        init: function($this, name) {
                            var elements = $this.parents("form").first().find('[name="' + $this.attr("name") + '"]');
                            return elements.bind("click.validation", function() {
                                $this.trigger("change.validation", {
                                    includeEmpty: !0
                                })
                            }), {
                                maxchecked: $this.data("validation" + name + "Maxchecked"),
                                elements: elements
                            }
                        },
                        validate: function($this, value, validator) {
                            return validator.elements.filter(":checked").length > validator.maxchecked && !validator.negative || validator.elements.filter(":checked").length <= validator.maxchecked && validator.negative
                        },
                        blockSubmit: !0
                    },
                    minchecked: {
                        name: "minchecked",
                        init: function($this, name) {
                            var elements = $this.parents("form").first().find('[name="' + $this.attr("name") + '"]');
                            return elements.bind("click.validation", function() {
                                $this.trigger("change.validation", {
                                    includeEmpty: !0
                                })
                            }), {
                                minchecked: $this.data("validation" + name + "Minchecked"),
                                elements: elements
                            }
                        },
                        validate: function($this, value, validator) {
                            return validator.elements.filter(":checked").length < validator.minchecked && !validator.negative || validator.elements.filter(":checked").length >= validator.minchecked && validator.negative
                        },
                        blockSubmit: !0
                    }
                },
                builtInValidators: {
                    email: {
                        name: "Email",
                        type: "shortcut",
                        shortcut: "validemail"
                    },
                    validemail: {
                        name: "Validemail",
                        type: "regex",
                        regex: "[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}",
                        message: "Not a valid email address<!-- data-validator-validemail-message to override -->"
                    },
                    passwordagain: {
                        name: "Passwordagain",
                        type: "match",
                        match: "password",
                        message: "Does not match the given password<!-- data-validator-paswordagain-message to override -->"
                    },
                    positive: {
                        name: "Positive",
                        type: "shortcut",
                        shortcut: "number,positivenumber"
                    },
                    negative: {
                        name: "Negative",
                        type: "shortcut",
                        shortcut: "number,negativenumber"
                    },
                    number: {
                        name: "Number",
                        type: "regex",
                        regex: "([+-]?\\d+(\\.\\d*)?([eE][+-]?[0-9]+)?)?",
                        message: "Must be a number<!-- data-validator-number-message to override -->"
                    },
                    integer: {
                        name: "Integer",
                        type: "regex",
                        regex: "[+-]?\\d+",
                        message: "No decimal places allowed<!-- data-validator-integer-message to override -->"
                    },
                    positivenumber: {
                        name: "Positivenumber",
                        type: "min",
                        min: 0,
                        message: "Must be a positive number<!-- data-validator-positivenumber-message to override -->"
                    },
                    negativenumber: {
                        name: "Negativenumber",
                        type: "max",
                        max: 0,
                        message: "Must be a negative number<!-- data-validator-negativenumber-message to override -->"
                    },
                    required: {
                        name: "Required",
                        type: "required",
                        message: "This is required<!-- data-validator-required-message to override -->"
                    },
                    checkone: {
                        name: "Checkone",
                        type: "minchecked",
                        minchecked: 1,
                        message: "Check at least one option<!-- data-validation-checkone-message to override -->"
                    }
                }
            },
            formatValidatorName = function(name) {
                return name.toLowerCase().replace(/(^|\s)([a-z])/g, function(m, p1, p2) {
                    return p1 + p2.toUpperCase()
                })
            },
            getValue = function($this) {
                var value = $this.val(),
                    type = $this.attr("type");
                return "checkbox" === type && (value = $this.is(":checked") ? value : ""), "radio" === type && (value = $('input[name="' + $this.attr("name") + '"]:checked').length > 0 ? value : ""), value
            };
        $.fn.jqBootstrapValidation = function(method) {
            return defaults.methods[method] ? defaults.methods[method].apply(this, Array.prototype.slice.call(arguments, 1)) : "object" != typeof method && method ? ($.error("Method " + method + " does not exist on jQuery.jqBootstrapValidation"), null) : defaults.methods.init.apply(this, arguments)
        }, $.jqBootstrapValidation = function() {
            $(":input").not("[type=image],[type=submit]").jqBootstrapValidation.apply(this, arguments)
        }
    }(jQuery),//CONTROL DATAPICKER - CALENDARIO
    function($, undefined) {
        function UTCDate() {
            return new Date(Date.UTC.apply(Date, arguments))
        }

        function UTCToday() {
            var today = new Date;
            return UTCDate(today.getFullYear(), today.getMonth(), today.getDate())
        }

        function isUTCEquals(date1, date2) {
            return date1.getUTCFullYear() === date2.getUTCFullYear() && date1.getUTCMonth() === date2.getUTCMonth() && date1.getUTCDate() === date2.getUTCDate()
        }

        function alias(method) {
            return function() {
                return this[method].apply(this, arguments)
            }
        }

        function opts_from_el(el, prefix) {
            function re_lower(_, a) {
                return a.toLowerCase()
            }
            var inkey, data = $(el).data(),
                out = {},
                replace = new RegExp("^" + prefix.toLowerCase() + "([A-Z])");
            prefix = new RegExp("^" + prefix.toLowerCase());
            for (var key in data) prefix.test(key) && (inkey = key.replace(replace, re_lower), out[inkey] = data[key]);
            return out
        }

        function opts_from_locale(lang) {
            var out = {};
            if (dates[lang] || (lang = lang.split("-")[0], dates[lang])) {
                var d = dates[lang];
                return $.each(locale_opts, function(i, k) {
                    k in d && (out[k] = d[k])
                }), out
            }
        }
        var DateArray = function() {
                var extras = {
                    get: function(i) {
                        return this.slice(i)[0]
                    },
                    contains: function(d) {
                        for (var val = d && d.valueOf(), i = 0, l = this.length; l > i; i++)
                            if (this[i].valueOf() === val) return i;
                        return -1
                    },
                    remove: function(i) {
                        this.splice(i, 1)
                    },
                    replace: function(new_array) {
                        new_array && ($.isArray(new_array) || (new_array = [new_array]), this.clear(), this.push.apply(this, new_array))
                    },
                    clear: function() {
                        this.length = 0
                    },
                    copy: function() {
                        var a = new DateArray;
                        return a.replace(this), a
                    }
                };
                return function() {
                    var a = [];
                    return a.push.apply(a, arguments), $.extend(a, extras), a
                }
            }(),
            Datepicker = function(element, options) {
                this._process_options(options), this.dates = new DateArray, this.viewDate = this.o.defaultViewDate, this.focusDate = null, this.element = $(element), this.isInline = !1, this.isInput = this.element.is("input"), this.component = this.element.hasClass("date") ? this.element.find(".add-on, .input-group-addon, .btn") : !1, this.hasInput = this.component && this.element.find("input").length, this.component && 0 === this.component.length && (this.component = !1), this.picker = $(DPGlobal.template), this._buildEvents(), this._attachEvents(), this.isInline ? this.picker.addClass("datepicker-inline").appendTo(this.element) : this.picker.addClass("datepicker-dropdown dropdown-menu"), this.o.rtl && this.picker.addClass("datepicker-rtl"), this.viewMode = this.o.startView, this.o.calendarWeeks && this.picker.find("tfoot .today, tfoot .clear").attr("colspan", function(i, val) {
                    return parseInt(val) + 1
                }), this._allow_update = !1, this.setStartDate(this._o.startDate), this.setEndDate(this._o.endDate), this.setDaysOfWeekDisabled(this.o.daysOfWeekDisabled), this.setDatesDisabled(this.o.datesDisabled), this.fillDow(), this.fillMonths(), this._allow_update = !0, this.update(), this.showMode(), this.isInline && this.show()
            };
        Datepicker.prototype = {
            constructor: Datepicker,
            _process_options: function(opts) {
                this._o = $.extend({}, this._o, opts);
                var o = this.o = $.extend({}, this._o),
                    lang = o.language;
                switch (dates[lang] || (lang = lang.split("-")[0], dates[lang] || (lang = defaults.language)), o.language = lang, o.startView) {
                    case 2:
                    case "decade":
                        o.startView = 2;
                        break;
                    case 1:
                    case "year":
                        o.startView = 1;
                        break;
                    default:
                        o.startView = 0
                }
                switch (o.minViewMode) {
                    case 1:
                    case "months":
                        o.minViewMode = 1;
                        break;
                    case 2:
                    case "years":
                        o.minViewMode = 2;
                        break;
                    default:
                        o.minViewMode = 0
                }
                o.startView = Math.max(o.startView, o.minViewMode), o.multidate !== !0 && (o.multidate = Number(o.multidate) || !1, o.multidate !== !1 && (o.multidate = Math.max(0, o.multidate))), o.multidateSeparator = String(o.multidateSeparator), o.weekStart %= 7, o.weekEnd = (o.weekStart + 6) % 7;
                var format = DPGlobal.parseFormat(o.format);
                if (o.startDate !== -1 / 0 && (o.startDate = o.startDate ? o.startDate instanceof Date ? this._local_to_utc(this._zero_time(o.startDate)) : DPGlobal.parseDate(o.startDate, format, o.language) : -1 / 0), 1 / 0 !== o.endDate && (o.endDate = o.endDate ? o.endDate instanceof Date ? this._local_to_utc(this._zero_time(o.endDate)) : DPGlobal.parseDate(o.endDate, format, o.language) : 1 / 0), o.daysOfWeekDisabled = o.daysOfWeekDisabled || [], $.isArray(o.daysOfWeekDisabled) || (o.daysOfWeekDisabled = o.daysOfWeekDisabled.split(/[,\s]*/)), o.daysOfWeekDisabled = $.map(o.daysOfWeekDisabled, function(d) {
                        return parseInt(d, 10)
                    }), o.datesDisabled = o.datesDisabled || [], !$.isArray(o.datesDisabled)) {
                    var datesDisabled = [];
                    datesDisabled.push(DPGlobal.parseDate(o.datesDisabled, format, o.language)), o.datesDisabled = datesDisabled
                }
                o.datesDisabled = $.map(o.datesDisabled, function(d) {
                    return DPGlobal.parseDate(d, format, o.language)
                });
                var plc = String(o.orientation).toLowerCase().split(/\s+/g),
                    _plc = o.orientation.toLowerCase();
                if (plc = $.grep(plc, function(word) {
                        return /^auto|left|right|top|bottom$/.test(word)
                    }), o.orientation = {
                        x: "auto",
                        y: "auto"
                    }, _plc && "auto" !== _plc)
                    if (1 === plc.length) switch (plc[0]) {
                        case "top":
                        case "bottom":
                            o.orientation.y = plc[0];
                            break;
                        case "left":
                        case "right":
                            o.orientation.x = plc[0]
                    } else _plc = $.grep(plc, function(word) {
                        return /^left|right$/.test(word)
                    }), o.orientation.x = _plc[0] || "auto", _plc = $.grep(plc, function(word) {
                        return /^top|bottom$/.test(word)
                    }), o.orientation.y = _plc[0] || "auto";
                    else;
                if (o.defaultViewDate) {
                    var year = o.defaultViewDate.year || (new Date).getFullYear(),
                        month = o.defaultViewDate.month || 0,
                        day = o.defaultViewDate.day || 1;
                    o.defaultViewDate = UTCDate(year, month, day)
                } else o.defaultViewDate = UTCToday();
                o.showOnFocus = o.showOnFocus !== undefined ? o.showOnFocus : !0
            },
            _events: [],
            _secondaryEvents: [],
            _applyEvents: function(evs) {
                for (var el, ch, ev, i = 0; i < evs.length; i++) el = evs[i][0], 2 === evs[i].length ? (ch = undefined, ev = evs[i][1]) : 3 === evs[i].length && (ch = evs[i][1], ev = evs[i][2]), el.on(ev, ch)
            },
            _unapplyEvents: function(evs) {
                for (var el, ev, ch, i = 0; i < evs.length; i++) el = evs[i][0], 2 === evs[i].length ? (ch = undefined, ev = evs[i][1]) : 3 === evs[i].length && (ch = evs[i][1], ev = evs[i][2]), el.off(ev, ch)
            },
            _buildEvents: function() {
                var events = {
                    keyup: $.proxy(function(e) {
                        -1 === $.inArray(e.keyCode, [27, 37, 39, 38, 40, 32, 13, 9]) && this.update()
                    }, this),
                    keydown: $.proxy(this.keydown, this),
                    paste: $.proxy(this.paste, this)
                };
                this.o.showOnFocus === !0 && (events.focus = $.proxy(this.show, this)), this.isInput ? this._events = [
                    [this.element, events]
                ] : this.component && this.hasInput ? this._events = [
                    [this.element.find("input"), events],
                    [this.component, {
                        click: $.proxy(this.show, this)
                    }]
                ] : this.element.is("div") ? this.isInline = !0 : this._events = [
                    [this.element, {
                        click: $.proxy(this.show, this)
                    }]
                ], this._events.push([this.element, "*", {
                    blur: $.proxy(function(e) {
                        this._focused_from = e.target
                    }, this)
                }], [this.element, {
                    blur: $.proxy(function(e) {
                        this._focused_from = e.target
                    }, this)
                }]), this.o.immediateUpdates && this._events.push([this.element, {
                    "changeYear changeMonth": $.proxy(function(e) {
                        this.update(e.date)
                    }, this)
                }]), this._secondaryEvents = [
                    [this.picker, {
                        click: $.proxy(this.click, this)
                    }],
                    [$(window), {
                        resize: $.proxy(this.place, this)
                    }],
                    [$(document), {
                        mousedown: $.proxy(function(e) {
                            this.element.is(e.target) || this.element.find(e.target).length || this.picker.is(e.target) || this.picker.find(e.target).length || $(this.picker).hide()
                        }, this)
                    }]
                ]
            },
            _attachEvents: function() {
                this._detachEvents(), this._applyEvents(this._events)
            },
            _detachEvents: function() {
                this._unapplyEvents(this._events)
            },
            _attachSecondaryEvents: function() {
                this._detachSecondaryEvents(), this._applyEvents(this._secondaryEvents)
            },
            _detachSecondaryEvents: function() {
                this._unapplyEvents(this._secondaryEvents)
            },
            _trigger: function(event, altdate) {
                var date = altdate || this.dates.get(-1),
                    local_date = this._utc_to_local(date);
                this.element.trigger({
                    type: event,
                    date: local_date,
                    dates: $.map(this.dates, this._utc_to_local),
                    format: $.proxy(function(ix, format) {
                        0 === arguments.length ? (ix = this.dates.length - 1, format = this.o.format) : "string" == typeof ix && (format = ix, ix = this.dates.length - 1), format = format || this.o.format;
                        var date = this.dates.get(ix);
                        return DPGlobal.formatDate(date, format, this.o.language)
                    }, this)
                })
            },
            show: function() {
                return this.element.attr("readonly") && this.o.enableOnReadonly === !1 ? void 0 : (this.isInline || this.picker.appendTo(this.o.container), this.place(), this.picker.show(), this._attachSecondaryEvents(), this._trigger("show"), (window.navigator.msMaxTouchPoints || "ontouchstart" in document) && this.o.disableTouchKeyboard && $(this.element).blur(), this)
            },
            hide: function() {
                return this.isInline ? this : this.picker.is(":visible") ? (this.focusDate = null, this.picker.hide().detach(), this._detachSecondaryEvents(), this.viewMode = this.o.startView, this.showMode(), this.o.forceParse && (this.isInput && this.element.val() || this.hasInput && this.element.find("input").val()) && this.setValue(), this._trigger("hide"), this) : this
            },
            remove: function() {
                return this.hide(), this._detachEvents(), this._detachSecondaryEvents(), this.picker.remove(), delete this.element.data().datepicker, this.isInput || delete this.element.data().date, this
            },
            paste: function(evt) {
                var dateString;
                if (evt.originalEvent.clipboardData && evt.originalEvent.clipboardData.types && -1 !== $.inArray("text/plain", evt.originalEvent.clipboardData.types)) dateString = evt.originalEvent.clipboardData.getData("text/plain");
                else {
                    if (!window.clipboardData) return;
                    dateString = window.clipboardData.getData("Text")
                }
                this.setDate(dateString), this.update(), evt.preventDefault()
            },
            _utc_to_local: function(utc) {
                return utc && new Date(utc.getTime() + 6e4 * utc.getTimezoneOffset())
            },
            _local_to_utc: function(local) {
                return local && new Date(local.getTime() - 6e4 * local.getTimezoneOffset())
            },
            _zero_time: function(local) {
                return local && new Date(local.getFullYear(), local.getMonth(), local.getDate())
            },
            _zero_utc_time: function(utc) {
                return utc && new Date(Date.UTC(utc.getUTCFullYear(), utc.getUTCMonth(), utc.getUTCDate()))
            },
            getDates: function() {
                return $.map(this.dates, this._utc_to_local)
            },
            getUTCDates: function() {
                return $.map(this.dates, function(d) {
                    return new Date(d)
                })
            },
            getDate: function() {
                return this._utc_to_local(this.getUTCDate())
            },
            getUTCDate: function() {
                var selected_date = this.dates.get(-1);
                return "undefined" != typeof selected_date ? new Date(selected_date) : null
            },
            clearDates: function() {
                var element;
                this.isInput ? element = this.element : this.component && (element = this.element.find("input")), element && element.val("").change(), this.update(), this._trigger("changeDate"), this.o.autoclose && this.hide()
            },
            setDates: function() {
                var args = $.isArray(arguments[0]) ? arguments[0] : arguments;
                return this.update.apply(this, args), this._trigger("changeDate"), this.setValue(), this
            },
            setUTCDates: function() {
                var args = $.isArray(arguments[0]) ? arguments[0] : arguments;
                return this.update.apply(this, $.map(args, this._utc_to_local)), this._trigger("changeDate"), this.setValue(), this
            },
            setDate: alias("setDates"),
            setUTCDate: alias("setUTCDates"),
            setValue: function() {
                var formatted = this.getFormattedDate();
                return this.isInput ? this.element.val(formatted).change() : this.component && this.element.find("input").val(formatted).change(), this
            },
            getFormattedDate: function(format) {
                format === undefined && (format = this.o.format);
                var lang = this.o.language;
                return $.map(this.dates, function(d) {
                    return DPGlobal.formatDate(d, format, lang)
                }).join(this.o.multidateSeparator)
            },
            setStartDate: function(startDate) {
                return this._process_options({
                    startDate: startDate
                }), this.update(), this.updateNavArrows(), this
            },
            setEndDate: function(endDate) {
                return this._process_options({
                    endDate: endDate
                }), this.update(), this.updateNavArrows(), this
            },
            setDaysOfWeekDisabled: function(daysOfWeekDisabled) {
                return this._process_options({
                    daysOfWeekDisabled: daysOfWeekDisabled
                }), this.update(), this.updateNavArrows(), this
            },
            setDatesDisabled: function(datesDisabled) {
                this._process_options({
                    datesDisabled: datesDisabled
                }), this.update(), this.updateNavArrows()
            },
            place: function() {
                if (this.isInline) return this;
                var calendarWidth = this.picker.outerWidth(),
                    calendarHeight = this.picker.outerHeight(),
                    visualPadding = 10,
                    windowWidth = $(this.o.container).width(),
                    windowHeight = $(this.o.container).height(),
                    scrollTop = $(this.o.container).scrollTop(),
                    appendOffset = $(this.o.container).offset(),
                    parentsZindex = [];
                this.element.parents().each(function() {
                    var itemZIndex = $(this).css("z-index");
                    "auto" !== itemZIndex && 0 !== itemZIndex && parentsZindex.push(parseInt(itemZIndex))
                });
                var zIndex = Math.max.apply(Math, parentsZindex) + 10,
                    offset = this.component ? this.component.parent().offset() : this.element.offset(),
                    height = this.component ? this.component.outerHeight(!0) : this.element.outerHeight(!1),
                    width = this.component ? this.component.outerWidth(!0) : this.element.outerWidth(!1),
                    left = offset.left - appendOffset.left,
                    top = offset.top - appendOffset.top;
                this.picker.removeClass("datepicker-orient-top datepicker-orient-bottom datepicker-orient-right datepicker-orient-left"), "auto" !== this.o.orientation.x ? (this.picker.addClass("datepicker-orient-" + this.o.orientation.x), "right" === this.o.orientation.x && (left -= calendarWidth - width)) : offset.left < 0 ? (this.picker.addClass("datepicker-orient-left"), left -= offset.left - visualPadding) : left + calendarWidth > windowWidth ? (this.picker.addClass("datepicker-orient-right"), left = offset.left + width - calendarWidth) : this.picker.addClass("datepicker-orient-left");
                var top_overflow, bottom_overflow, yorient = this.o.orientation.y;
                if ("auto" === yorient && (top_overflow = -scrollTop + top - calendarHeight, bottom_overflow = scrollTop + windowHeight - (top + height + calendarHeight), yorient = Math.max(top_overflow, bottom_overflow) === bottom_overflow ? "top" : "bottom"), this.picker.addClass("datepicker-orient-" + yorient), "top" === yorient ? top += height : top -= calendarHeight + parseInt(this.picker.css("padding-top")), this.o.rtl) {
                    var right = windowWidth - (left + width);
                    this.picker.css({
                        top: top,
                        right: right,
                        zIndex: zIndex
                    })
                } else this.picker.css({
                    top: top,
                    left: left,
                    zIndex: zIndex
                });
                return this
            },
            _allow_update: !0,
            update: function() {
                if (!this._allow_update) return this;
                var oldDates = this.dates.copy(),
                    dates = [],
                    fromArgs = !1;
                return arguments.length ? ($.each(arguments, $.proxy(function(i, date) {
                    date instanceof Date && (date = this._local_to_utc(date)), dates.push(date)
                }, this)), fromArgs = !0) : (dates = this.isInput ? this.element.val() : this.element.data("date") || this.element.find("input").val(), dates = dates && this.o.multidate ? dates.split(this.o.multidateSeparator) : [dates], delete this.element.data().date), dates = $.map(dates, $.proxy(function(date) {
                    return DPGlobal.parseDate(date, this.o.format, this.o.language)
                }, this)), dates = $.grep(dates, $.proxy(function(date) {
                    return date < this.o.startDate || date > this.o.endDate || !date
                }, this), !0), this.dates.replace(dates), this.dates.length ? this.viewDate = new Date(this.dates.get(-1)) : this.viewDate < this.o.startDate ? this.viewDate = new Date(this.o.startDate) : this.viewDate > this.o.endDate && (this.viewDate = new Date(this.o.endDate)), fromArgs ? this.setValue() : dates.length && String(oldDates) !== String(this.dates) && this._trigger("changeDate"), !this.dates.length && oldDates.length && this._trigger("clearDate"), this.fill(), this
            },
            fillDow: function() {
                var dowCnt = this.o.weekStart,
                    html = "<tr>";
                if (this.o.calendarWeeks) {
                    this.picker.find(".datepicker-days thead tr:first-child .datepicker-switch").attr("colspan", function(i, val) {
                        return parseInt(val) + 1
                    });
                    var cell = '<th class="cw">&#160;</th>';
                    html += cell
                }
                for (; dowCnt < this.o.weekStart + 7;) html += '<th class="dow">' + dates[this.o.language].daysMin[dowCnt++ % 7] + "</th>";
                html += "</tr>", this.picker.find(".datepicker-days thead").append(html)
            },
            fillMonths: function() {
                for (var html = "", i = 0; 12 > i;) html += '<span class="month">' + dates[this.o.language].monthsShort[i++] + "</span>";
                this.picker.find(".datepicker-months td").html(html)
            },
            setRange: function(range) {
                range && range.length ? this.range = $.map(range, function(d) {
                    return d.valueOf()
                }) : delete this.range, this.fill()
            },
            getClassNames: function(date) {
                var cls = [],
                    year = this.viewDate.getUTCFullYear(),
                    month = this.viewDate.getUTCMonth(),
                    today = new Date;
                return date.getUTCFullYear() < year || date.getUTCFullYear() === year && date.getUTCMonth() < month ? cls.push("old") : (date.getUTCFullYear() > year || date.getUTCFullYear() === year && date.getUTCMonth() > month) && cls.push("new"), this.focusDate && date.valueOf() === this.focusDate.valueOf() && cls.push("focused"), this.o.todayHighlight && date.getUTCFullYear() === today.getFullYear() && date.getUTCMonth() === today.getMonth() && date.getUTCDate() === today.getDate() && cls.push("today"), -1 !== this.dates.contains(date) && cls.push("active"), (date.valueOf() < this.o.startDate || date.valueOf() > this.o.endDate || -1 !== $.inArray(date.getUTCDay(), this.o.daysOfWeekDisabled)) && cls.push("disabled"), this.o.datesDisabled.length > 0 && $.grep(this.o.datesDisabled, function(d) {
                    return isUTCEquals(date, d)
                }).length > 0 && cls.push("disabled", "disabled-date"), this.range && (date > this.range[0] && date < this.range[this.range.length - 1] && cls.push("range"), -1 !== $.inArray(date.valueOf(), this.range) && cls.push("selected")), cls
            },
            fill: function() {
                var tooltip, d = new Date(this.viewDate),
                    year = d.getUTCFullYear(),
                    month = d.getUTCMonth(),
                    startYear = this.o.startDate !== -1 / 0 ? this.o.startDate.getUTCFullYear() : -1 / 0,
                    startMonth = this.o.startDate !== -1 / 0 ? this.o.startDate.getUTCMonth() : -1 / 0,
                    endYear = 1 / 0 !== this.o.endDate ? this.o.endDate.getUTCFullYear() : 1 / 0,
                    endMonth = 1 / 0 !== this.o.endDate ? this.o.endDate.getUTCMonth() : 1 / 0,
                    todaytxt = dates[this.o.language].today || dates.en.today || "",
                    cleartxt = dates[this.o.language].clear || dates.en.clear || "";
                if (!isNaN(year) && !isNaN(month)) {
                    this.picker.find(".datepicker-days thead .datepicker-switch").text(dates[this.o.language].months[month] + " " + year), this.picker.find("tfoot .today").text(todaytxt).toggle(this.o.todayBtn !== !1), this.picker.find("tfoot .clear").text(cleartxt).toggle(this.o.clearBtn !== !1), this.updateNavArrows(), this.fillMonths();
                    var prevMonth = UTCDate(year, month - 1, 28),
                        day = DPGlobal.getDaysInMonth(prevMonth.getUTCFullYear(), prevMonth.getUTCMonth());
                    prevMonth.setUTCDate(day), prevMonth.setUTCDate(day - (prevMonth.getUTCDay() - this.o.weekStart + 7) % 7);
                    var nextMonth = new Date(prevMonth);
                    nextMonth.setUTCDate(nextMonth.getUTCDate() + 42), nextMonth = nextMonth.valueOf();
                    for (var clsName, html = []; prevMonth.valueOf() < nextMonth;) {
                        if (prevMonth.getUTCDay() === this.o.weekStart && (html.push("<tr>"), this.o.calendarWeeks)) {
                            var ws = new Date(+prevMonth + (this.o.weekStart - prevMonth.getUTCDay() - 7) % 7 * 864e5),
                                th = new Date(Number(ws) + (11 - ws.getUTCDay()) % 7 * 864e5),
                                yth = new Date(Number(yth = UTCDate(th.getUTCFullYear(), 0, 1)) + (11 - yth.getUTCDay()) % 7 * 864e5),
                                calWeek = (th - yth) / 864e5 / 7 + 1;
                            html.push('<td class="cw">' + calWeek + "</td>")
                        }
                        if (clsName = this.getClassNames(prevMonth), clsName.push("day"), this.o.beforeShowDay !== $.noop) {
                            var before = this.o.beforeShowDay(this._utc_to_local(prevMonth));
                            before === undefined ? before = {} : "boolean" == typeof before ? before = {
                                enabled: before
                            } : "string" == typeof before && (before = {
                                classes: before
                            }), before.enabled === !1 && clsName.push("disabled"), before.classes && (clsName = clsName.concat(before.classes.split(/\s+/))), before.tooltip && (tooltip = before.tooltip)
                        }
                        clsName = $.unique(clsName), html.push('<td class="' + clsName.join(" ") + '"' + (tooltip ? ' title="' + tooltip + '"' : "") + ">" + prevMonth.getUTCDate() + "</td>"), tooltip = null, prevMonth.getUTCDay() === this.o.weekEnd && html.push("</tr>"), prevMonth.setUTCDate(prevMonth.getUTCDate() + 1)
                    }
                    this.picker.find(".datepicker-days tbody").empty().append(html.join(""));
                    var months = this.picker.find(".datepicker-months").find("th:eq(1)").text(year).end().find("span").removeClass("active");
                    if ($.each(this.dates, function(i, d) {
                            d.getUTCFullYear() === year && months.eq(d.getUTCMonth()).addClass("active")
                        }), (startYear > year || year > endYear) && months.addClass("disabled"), year === startYear && months.slice(0, startMonth).addClass("disabled"), year === endYear && months.slice(endMonth + 1).addClass("disabled"), this.o.beforeShowMonth !== $.noop) {
                        var that = this;
                        $.each(months, function(i, month) {
                            if (!$(month).hasClass("disabled")) {
                                var moDate = new Date(year, i, 1),
                                    before = that.o.beforeShowMonth(moDate);
                                before === !1 && $(month).addClass("disabled")
                            }
                        })
                    }
                    html = "", year = 10 * parseInt(year / 10, 10);
                    var yearCont = this.picker.find(".datepicker-years").find("th:eq(1)").text(year + "-" + (year + 9)).end().find("td");
                    year -= 1;
                    for (var classes, years = $.map(this.dates, function(d) {
                            return d.getUTCFullYear()
                        }), i = -1; 11 > i; i++) classes = ["year"], -1 === i ? classes.push("old") : 10 === i && classes.push("new"), -1 !== $.inArray(year, years) && classes.push("active"), (startYear > year || year > endYear) && classes.push("disabled"), html += '<span class="' + classes.join(" ") + '">' + year + "</span>", year += 1;
                    yearCont.html(html)
                }
            },
            updateNavArrows: function() {
                if (this._allow_update) {
                    var d = new Date(this.viewDate),
                        year = d.getUTCFullYear(),
                        month = d.getUTCMonth();
                    switch (this.viewMode) {
                        case 0:
                            this.picker.find(".prev").css(this.o.startDate !== -1 / 0 && year <= this.o.startDate.getUTCFullYear() && month <= this.o.startDate.getUTCMonth() ? {
                                visibility: "hidden"
                            } : {
                                visibility: "visible"
                            }), this.picker.find(".next").css(1 / 0 !== this.o.endDate && year >= this.o.endDate.getUTCFullYear() && month >= this.o.endDate.getUTCMonth() ? {
                                visibility: "hidden"
                            } : {
                                visibility: "visible"
                            });
                            break;
                        case 1:
                        case 2:
                            this.picker.find(".prev").css(this.o.startDate !== -1 / 0 && year <= this.o.startDate.getUTCFullYear() ? {
                                visibility: "hidden"
                            } : {
                                visibility: "visible"
                            }), this.picker.find(".next").css(1 / 0 !== this.o.endDate && year >= this.o.endDate.getUTCFullYear() ? {
                                visibility: "hidden"
                            } : {
                                visibility: "visible"
                            })
                    }
                }
            },
            click: function(e) {
                e.preventDefault();
                var year, month, day, target = $(e.target).closest("span, td, th");
                if (1 === target.length) switch (target[0].nodeName.toLowerCase()) {
                    case "th":
                        switch (target[0].className) {
                            case "datepicker-switch":
                                this.showMode(1);
                                break;
                            case "prev":
                            case "next":
                                var dir = DPGlobal.modes[this.viewMode].navStep * ("prev" === target[0].className ? -1 : 1);
                                switch (this.viewMode) {
                                    case 0:
                                        this.viewDate = this.moveMonth(this.viewDate, dir), this._trigger("changeMonth", this.viewDate);
                                        break;
                                    case 1:
                                    case 2:
                                        this.viewDate = this.moveYear(this.viewDate, dir), 1 === this.viewMode && this._trigger("changeYear", this.viewDate)
                                }
                                this.fill();
                                break;
                            case "today":
                                var date = new Date;
                                date = UTCDate(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0), this.showMode(-2);
                                var which = "linked" === this.o.todayBtn ? null : "view";
                                this._setDate(date, which);
                                break;
                            case "clear":
                                this.clearDates()
                        }
                        break;
                    case "span":
                        target.hasClass("disabled") || (this.viewDate.setUTCDate(1), target.hasClass("month") ? (day = 1, month = target.parent().find("span").index(target), year = this.viewDate.getUTCFullYear(), this.viewDate.setUTCMonth(month), this._trigger("changeMonth", this.viewDate), 1 === this.o.minViewMode ? (this._setDate(UTCDate(year, month, day)), this.showMode()) : this.showMode(-1)) : (day = 1, month = 0, year = parseInt(target.text(), 10) || 0, this.viewDate.setUTCFullYear(year), this._trigger("changeYear", this.viewDate), 2 === this.o.minViewMode && this._setDate(UTCDate(year, month, day)), this.showMode(-1)), this.fill());
                        break;
                    case "td":
                        target.hasClass("day") && !target.hasClass("disabled") && (day = parseInt(target.text(), 10) || 1, year = this.viewDate.getUTCFullYear(), month = this.viewDate.getUTCMonth(), target.hasClass("old") ? 0 === month ? (month = 11, year -= 1) : month -= 1 : target.hasClass("new") && (11 === month ? (month = 0, year += 1) : month += 1), this._setDate(UTCDate(year, month, day)))
                }
                this.picker.is(":visible") && this._focused_from && $(this._focused_from).focus(), delete this._focused_from
            },
            _toggle_multidate: function(date) {
                var ix = this.dates.contains(date);
                if (date || this.dates.clear(), -1 !== ix ? (this.o.multidate === !0 || this.o.multidate > 1 || this.o.toggleActive) && this.dates.remove(ix) : this.o.multidate === !1 ? (this.dates.clear(), this.dates.push(date)) : this.dates.push(date), "number" == typeof this.o.multidate)
                    for (; this.dates.length > this.o.multidate;) this.dates.remove(0)
            },
            _setDate: function(date, which) {
                which && "date" !== which || this._toggle_multidate(date && new Date(date)), which && "view" !== which || (this.viewDate = date && new Date(date)), this.fill(), this.setValue(), which && "view" === which || this._trigger("changeDate");
                var element;
                this.isInput ? element = this.element : this.component && (element = this.element.find("input")), element && element.change(), !this.o.autoclose || which && "date" !== which || this.hide()
            },
            moveMonth: function(date, dir) {
                if (!date) return undefined;
                if (!dir) return date;
                var new_month, test, new_date = new Date(date.valueOf()),
                    day = new_date.getUTCDate(),
                    month = new_date.getUTCMonth(),
                    mag = Math.abs(dir);
                if (dir = dir > 0 ? 1 : -1, 1 === mag) test = -1 === dir ? function() {
                    return new_date.getUTCMonth() === month
                } : function() {
                    return new_date.getUTCMonth() !== new_month
                }, new_month = month + dir, new_date.setUTCMonth(new_month), (0 > new_month || new_month > 11) && (new_month = (new_month + 12) % 12);
                else {
                    for (var i = 0; mag > i; i++) new_date = this.moveMonth(new_date, dir);
                    new_month = new_date.getUTCMonth(), new_date.setUTCDate(day), test = function() {
                        return new_month !== new_date.getUTCMonth()
                    }
                }
                for (; test();) new_date.setUTCDate(--day), new_date.setUTCMonth(new_month);
                return new_date
            },
            moveYear: function(date, dir) {
                return this.moveMonth(date, 12 * dir)
            },
            dateWithinRange: function(date) {
                return date >= this.o.startDate && date <= this.o.endDate
            },
            keydown: function(e) {
                if (!this.picker.is(":visible")) return void((40 === e.keyCode || 27 === e.keyCode) && this.show());
                var dir, newDate, newViewDate, dateChanged = !1,
                    focusDate = this.focusDate || this.viewDate;
                switch (e.keyCode) {
                    case 27:
                        this.focusDate ? (this.focusDate = null, this.viewDate = this.dates.get(-1) || this.viewDate, this.fill()) : this.hide(), e.preventDefault();
                        break;
                    case 37:
                    case 39:
                        if (!this.o.keyboardNavigation) break;
                        dir = 37 === e.keyCode ? -1 : 1, e.ctrlKey ? (newDate = this.moveYear(this.dates.get(-1) || UTCToday(), dir), newViewDate = this.moveYear(focusDate, dir), this._trigger("changeYear", this.viewDate)) : e.shiftKey ? (newDate = this.moveMonth(this.dates.get(-1) || UTCToday(), dir), newViewDate = this.moveMonth(focusDate, dir), this._trigger("changeMonth", this.viewDate)) : (newDate = new Date(this.dates.get(-1) || UTCToday()), newDate.setUTCDate(newDate.getUTCDate() + dir), newViewDate = new Date(focusDate), newViewDate.setUTCDate(focusDate.getUTCDate() + dir)), this.dateWithinRange(newViewDate) && (this.focusDate = this.viewDate = newViewDate, this.setValue(), this.fill(), e.preventDefault());
                        break;
                    case 38:
                    case 40:
                        if (!this.o.keyboardNavigation) break;
                        dir = 38 === e.keyCode ? -1 : 1, e.ctrlKey ? (newDate = this.moveYear(this.dates.get(-1) || UTCToday(), dir), newViewDate = this.moveYear(focusDate, dir), this._trigger("changeYear", this.viewDate)) : e.shiftKey ? (newDate = this.moveMonth(this.dates.get(-1) || UTCToday(), dir), newViewDate = this.moveMonth(focusDate, dir), this._trigger("changeMonth", this.viewDate)) : (newDate = new Date(this.dates.get(-1) || UTCToday()), newDate.setUTCDate(newDate.getUTCDate() + 7 * dir), newViewDate = new Date(focusDate), newViewDate.setUTCDate(focusDate.getUTCDate() + 7 * dir)), this.dateWithinRange(newViewDate) && (this.focusDate = this.viewDate = newViewDate, this.setValue(), this.fill(), e.preventDefault());
                        break;
                    case 32:
                        break;
                    case 13:
                        focusDate = this.focusDate || this.dates.get(-1) || this.viewDate, this.o.keyboardNavigation && (this._toggle_multidate(focusDate), dateChanged = !0), this.focusDate = null, this.viewDate = this.dates.get(-1) || this.viewDate, this.setValue(), this.fill(), this.picker.is(":visible") && (e.preventDefault(), "function" == typeof e.stopPropagation ? e.stopPropagation() : e.cancelBubble = !0, this.o.autoclose && this.hide());
                        break;
                    case 9:
                        this.focusDate = null, this.viewDate = this.dates.get(-1) || this.viewDate, this.fill(), this.hide()
                }
                if (dateChanged) {
                    this._trigger(this.dates.length ? "changeDate" : "clearDate");
                    var element;
                    this.isInput ? element = this.element : this.component && (element = this.element.find("input")), element && element.change()
                }
            },
            showMode: function(dir) {
                dir && (this.viewMode = Math.max(this.o.minViewMode, Math.min(2, this.viewMode + dir))), this.picker.children("div").hide().filter(".datepicker-" + DPGlobal.modes[this.viewMode].clsName).css("display", "block"), this.updateNavArrows()
            }
        };
        var DateRangePicker = function(element, options) {
            this.element = $(element), this.inputs = $.map(options.inputs, function(i) {
                return i.jquery ? i[0] : i
            }), delete options.inputs, datepickerPlugin.call($(this.inputs), options).on("changeDate", $.proxy(this.dateUpdated, this)), this.pickers = $.map(this.inputs, function(i) {
                return $(i).data("datepicker")
            }), this.updateDates()
        };
        DateRangePicker.prototype = {
            updateDates: function() {
                this.dates = $.map(this.pickers, function(i) {
                    return i.getUTCDate()
                }), this.updateRanges()
            },
            updateRanges: function() {
                var range = $.map(this.dates, function(d) {
                    return d.valueOf()
                });
                $.each(this.pickers, function(i, p) {
                    p.setRange(range)
                })
            },
            dateUpdated: function(e) {
                if (!this.updating) {
                    this.updating = !0;
                    var dp = $(e.target).data("datepicker"),
                        new_date = dp.getUTCDate(),
                        i = $.inArray(e.target, this.inputs),
                        j = i - 1,
                        k = i + 1,
                        l = this.inputs.length;
                    if (-1 !== i) {
                        if ($.each(this.pickers, function(i, p) {
                                p.getUTCDate() || p.setUTCDate(new_date)
                            }), new_date < this.dates[j])
                            for (; j >= 0 && new_date < this.dates[j];) this.pickers[j--].setUTCDate(new_date);
                        else if (new_date > this.dates[k])
                            for (; l > k && new_date > this.dates[k];) this.pickers[k++].setUTCDate(new_date);
                        this.updateDates(), delete this.updating
                    }
                }
            },
            remove: function() {
                $.map(this.pickers, function(p) {
                    p.remove()
                }), delete this.element.data().datepicker
            }
        };
        var old = $.fn.datepicker,
            datepickerPlugin = function(option) {
                var args = Array.apply(null, arguments);
                args.shift();
                var internal_return;
                return this.each(function() {
                    var $this = $(this),
                        data = $this.data("datepicker"),
                        options = "object" == typeof option && option;
                    if (!data) {
                        var elopts = opts_from_el(this, "date"),
                            xopts = $.extend({}, defaults, elopts, options),
                            locopts = opts_from_locale(xopts.language),
                            opts = $.extend({}, defaults, locopts, elopts, options);
                        if ($this.hasClass("input-daterange") || opts.inputs) {
                            var ropts = {
                                inputs: opts.inputs || $this.find("input").toArray()
                            };
                            $this.data("datepicker", data = new DateRangePicker(this, $.extend(opts, ropts)))
                        } else $this.data("datepicker", data = new Datepicker(this, opts))
                    }
                    return "string" == typeof option && "function" == typeof data[option] && (internal_return = data[option].apply(data, args), internal_return !== undefined) ? !1 : void 0
                }), internal_return !== undefined ? internal_return : this
            };
        $.fn.datepicker = datepickerPlugin;
        var defaults = $.fn.datepicker.defaults = {
                autoclose: !1,
                beforeShowDay: $.noop,
                beforeShowMonth: $.noop,
                calendarWeeks: !1,
                clearBtn: !1,
                toggleActive: !1,
                daysOfWeekDisabled: [],
                datesDisabled: [],
                endDate: 1 / 0,
                forceParse: !0,
                format: "dd/mm/yyyy",
                keyboardNavigation: !0,
                language: "en",
                minViewMode: 0,
                multidate: !1,
                multidateSeparator: ",",
                orientation: "auto",
                rtl: !1,
                startDate: -1 / 0,
                startView: 0,
                todayBtn: !1,
                todayHighlight: !1,
                weekStart: 0,
                disableTouchKeyboard: !1,
                enableOnReadonly: !0,
                container: "body",
                immediateUpdates: !1
            },
            locale_opts = $.fn.datepicker.locale_opts = ["format", "rtl", "weekStart"];
        $.fn.datepicker.Constructor = Datepicker;
        var dates = $.fn.datepicker.dates = {
                en: {
                    days: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
                    daysShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
                    daysMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
                    months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Novimbre", "Diciembre"],
                    monthsShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                    today: "Today",
                    clear: "Clear"
                },
                es: {
                    days: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
                    daysShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
                    daysMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
                    months: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
                    monthsShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                    today: "Today",
                    clear: "Clear"
                }
            },
            DPGlobal = {
                modes: [{
                    clsName: "days",
                    navFnc: "Month",
                    navStep: 1
                }, {
                    clsName: "months",
                    navFnc: "FullYear",
                    navStep: 1
                }, {
                    clsName: "years",
                    navFnc: "FullYear",
                    navStep: 10
                }],
                isLeapYear: function(year) {
                    return year % 4 === 0 && year % 100 !== 0 || year % 400 === 0
                },
                getDaysInMonth: function(year, month) {
                    return [31, DPGlobal.isLeapYear(year) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month]
                },
                validParts: /dd?|DD?|mm?|MM?|yy(?:yy)?/g,
                nonpunctuation: /[^ -\/:-@\[\u3400-\u9fff-`{-~\t\n\r]+/g,
                parseFormat: function(format) {
                    var separators = format.replace(this.validParts, "\x00").split("\x00"),
                        parts = format.match(this.validParts);
                    if (!separators || !separators.length || !parts || 0 === parts.length) throw new Error("Invalid date format.");
                    return {
                        separators: separators,
                        parts: parts
                    }
                },
                parseDate: function(date, format, language) {
                    function match_part() {
                        var m = this.slice(0, parts[i].length),
                            p = parts[i].slice(0, m.length);
                        return m.toLowerCase() === p.toLowerCase()
                    }
                    if (!date) return undefined;
                    if (date instanceof Date) return date;
                    "string" == typeof format && (format = DPGlobal.parseFormat(format));
                    var part, dir, i, part_re = /([\-+]\d+)([dmwy])/,
                        parts = date.match(/([\-+]\d+)([dmwy])/g);
                    if (/^[\-+]\d+[dmwy]([\s,]+[\-+]\d+[dmwy])*$/.test(date)) {
                        for (date = new Date, i = 0; i < parts.length; i++) switch (part = part_re.exec(parts[i]), dir = parseInt(part[1]), part[2]) {
                            case "d":
                                date.setUTCDate(date.getUTCDate() + dir);
                                break;
                            case "m":
                                date = Datepicker.prototype.moveMonth.call(Datepicker.prototype, date, dir);
                                break;
                            case "w":
                                date.setUTCDate(date.getUTCDate() + 7 * dir);
                                break;
                            case "y":
                                date = Datepicker.prototype.moveYear.call(Datepicker.prototype, date, dir)
                        }
                        return UTCDate(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), 0, 0, 0)
                    }
                    parts = date && date.match(this.nonpunctuation) || [], date = new Date;
                    var val, filtered, parsed = {},
                        setters_order = ["yyyy", "yy", "M", "MM", "m", "mm", "d", "dd"],
                        setters_map = {
                            yyyy: function(d, v) {
                                return d.setUTCFullYear(v)
                            },
                            yy: function(d, v) {
                                return d.setUTCFullYear(2e3 + v)
                            },
                            m: function(d, v) {
                                if (isNaN(d)) return d;
                                for (v -= 1; 0 > v;) v += 12;
                                for (v %= 12, d.setUTCMonth(v); d.getUTCMonth() !== v;) d.setUTCDate(d.getUTCDate() - 1);
                                return d
                            },
                            d: function(d, v) {
                                return d.setUTCDate(v)
                            }
                        };
                    setters_map.M = setters_map.MM = setters_map.mm = setters_map.m, setters_map.dd = setters_map.d, date = UTCDate(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
                    var fparts = format.parts.slice();
                    if (parts.length !== fparts.length && (fparts = $(fparts).filter(function(i, p) {
                            return -1 !== $.inArray(p, setters_order)
                        }).toArray()), parts.length === fparts.length) {
                        var cnt;
                        for (i = 0, cnt = fparts.length; cnt > i; i++) {
                            if (val = parseInt(parts[i], 10), part = fparts[i], isNaN(val)) switch (part) {
                                case "MM":
                                    filtered = $(dates[language].months).filter(match_part), val = $.inArray(filtered[0], dates[language].months) + 1;
                                    break;
                                case "M":
                                    filtered = $(dates[language].monthsShort).filter(match_part), val = $.inArray(filtered[0], dates[language].monthsShort) + 1
                            }
                            parsed[part] = val
                        }
                        var _date, s;
                        for (i = 0; i < setters_order.length; i++) s = setters_order[i], s in parsed && !isNaN(parsed[s]) && (_date = new Date(date), setters_map[s](_date, parsed[s]), isNaN(_date) || (date = _date))
                    }
                    return date
                },
                formatDate: function(date, format, language) {
                    if (!date) return "";
                    "string" == typeof format && (format = DPGlobal.parseFormat(format));
                    var val = {
                        d: date.getUTCDate(),
                        D: dates[language].daysShort[date.getUTCDay()],
                        DD: dates[language].days[date.getUTCDay()],
                        m: date.getUTCMonth() + 1,
                        M: dates[language].monthsShort[date.getUTCMonth()],
                        MM: dates[language].months[date.getUTCMonth()],
                        yy: date.getUTCFullYear().toString().substring(2),
                        yyyy: date.getUTCFullYear()
                    };
                    val.dd = (val.d < 10 ? "0" : "") + val.d, val.mm = (val.m < 10 ? "0" : "") + val.m, date = [];
                    for (var seps = $.extend([], format.separators), i = 0, cnt = format.parts.length; cnt >= i; i++) seps.length && date.push(seps.shift()), date.push(val[format.parts[i]]);
                    return date.join("")
                },
                headTemplate: '<thead><tr><th class="prev">&#171;</th><th colspan="5" class="datepicker-switch"></th><th class="next">&#187;</th></tr></thead>',
                contTemplate: '<tbody><tr><td colspan="7"></td></tr></tbody>',
                footTemplate: '<tfoot><tr><th colspan="7" class="today"></th></tr><tr><th colspan="7" class="clear"></th></tr></tfoot>'
            };
        DPGlobal.template = '<div class="datepicker"><div class="datepicker-days"><table class=" table-condensed">' + DPGlobal.headTemplate + "<tbody></tbody>" + DPGlobal.footTemplate + '</table></div><div class="datepicker-months"><table class="table-condensed">' + DPGlobal.headTemplate + DPGlobal.contTemplate + DPGlobal.footTemplate + '</table></div><div class="datepicker-years"><table class="table-condensed">' + DPGlobal.headTemplate + DPGlobal.contTemplate + DPGlobal.footTemplate + "</table></div></div>", $.fn.datepicker.DPGlobal = DPGlobal, $.fn.datepicker.noConflict = function() {
            return $.fn.datepicker = old, this
        }, $.fn.datepicker.version = "1.4.1-dev", $(document).on("focus.datepicker.data-api click.datepicker.data-api", '[data-provide="datepicker"]', function(e) {
            var $this = $(this);
            $this.data("datepicker") || (e.preventDefault(), datepickerPlugin.call($this, "show"))
        }), $(function() {
            datepickerPlugin.call($('[data-provide="datepicker-inline"]'))
        })
    }(window.jQuery), ! function(a) {
        a.fn.datepicker.dates.es = {
            days: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
            daysShort: ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"],
            daysMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
            months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
            monthsShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
            today: "Hoy",
            clear: "Borrar",
            weekStart: 1,
            format: "dd/mm/yyyy"
        }
    }(jQuery), ! function(a) {
        a.fn.datepicker.dates["en-GB"] = {
            days: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            daysShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            daysMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
            months: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
            monthsShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            today: "Today",
            clear: "Clear",
            weekStart: 1,
            format: "dd/mm/yyyy"
        }
    }(jQuery), $(function() {
        $("button.navbar-toggle").click(function() {
            canvasTarget = $(this).attr("data-target"), dataSide = $(this).attr("data-side"), $(".canvas-wrap").addClass(dataSide), $(canvasTarget).hasClass("out") ? closeCanvas() : ($(".off-canvas").removeClass("out"), $("body").addClass("in").removeClass("left right").addClass(dataSide), $("html").addClass("in").removeClass("left right").addClass(dataSide), $(canvasTarget).addClass("out"), $(".canvas-wrap").show().css("height", "100%"))
        }), $(".canvas-wrap").click("click", function() {
            closeCanvas()
        }), $('.off-canvas [data-toggle="close"]').click("click", function() {
            closeCanvas()
        })
    }), $(function() {
        $('.mobile-menu-toggle').click(function(e) {
            $('body').toggleClass('enable-mobile-menu'), $('body').removeClass('scroll-mobile-menu')
        }), $('.mobile-menu').scroll(function(e) {
            $('body').addClass('scroll-mobile-menu')
        })
    }), $(function() {
        $(".js-responsive-auto-close").collapse(mdBreak >= windowWidth ? {
            toggle: !0
        } : {
            toggle: !1
        }), $(".js-responsive-auto-close-xs").collapse(xsBreak > windowWidth ? {
            toggle: !0
        } : {
            toggle: !1
        }), $('[data-toggle="html-popover"]').popover({
            html: !0,
            content: function() {
                var popContentID = $(this).attr("data-id-content");
                return $(popContentID).html()
            }
        }), $('[data-toggle="html-popover"]').click(function() {
            $('[data-toggle="html-popover"]').not(this).popover("hide")
        }), $("a,button").click(function() {
            originalText = $(this).attr("data-original-text"), newText = $(this).attr("data-new-text"), $(this).attr("data-original-text", newText), $(this).attr("data-new-text", originalText), $(this).html(newText)
        }), $('[data-toggle="collapse"][aria-expanded="false"]').on("click", function() {
            hRef = $(this).attr("href"), hRef || (hRef = $(this).attr("data-target")), thisHeight = $(this).parent().height(), $(this).hasClass("no-scroll") || ($(hRef).on("show.bs.collapse", function() {
                $("html").addClass("scroll-stop")
            }), $(hRef).on("shown.bs.collapse", function() {
                moveToOffset = $(this).offset().top - thisHeight - 10, $("html, body").animate({
                    scrollTop: moveToOffset,
                    useTranslate3d: !0
                }, 700), $(window).scroll(function() {
                    clearTimeout($.data(this, "scrollCheck")), $.data(this, "scrollCheck", setTimeout(function() {
                        $("html").removeClass("scroll-stop")
                    }, 250))
                })
            }))
        }), $("[data-id-scroll]").on("click", function() {
            function waitUntilVisible() {
                existInterval = !1, $(scrollToID).hasClass("in") ? (moveToOffset = $(scrollToID).offset().top - parseInt(pixelFix), $("html, body").animate({
                    scrollTop: moveToOffset,
                    useTranslate3d: !0
                }, 700), existInterval === !0 && clearInterval(visibleInterval)) : (existInterval = !0, visibleInterval = setInterval(waituntilVisible, 500))
            }
            return scrollToID = $(this).attr("data-id-scroll"), pixelFix = $(this).attr("data-pixel-fix"), waitUntilVisible(), $(this).is("a") ? !1 : void 0
        }), $("[type='number']").keydown(function(event) {
            event.keyCode > 90 && event.keyCode < 106 || 46 == event.keyCode || 8 == event.keyCode || 9 == event.keyCode || (event.keyCode < 48 || event.keyCode > 57) && event.preventDefault()
        }), $("[data-id-dismiss]").click(function() {
            idToClose = $(this).attr("data-id-dismiss"), idToClose && ("this" === idToClose && (idToClose = $(this).parent()), exitAnimation = $(this).attr("data-animation"), $(idToClose).addClass("animated").addClass(exitAnimation), $(idToClose).one("webkitAnimationEnd oanimationend msAnimationEnd animationend", function() {
                $(this).addClass("hide")
            }))
        }), $(".js-reveal").click(function() {
            idToShow = $(this).attr("data-id-show"), idToHide = $(this).attr("data-id-hide"), showAnimation = $(this).attr("data-animation-show"), hideAnimation = $(this).attr("data-animation-hide"), idToHide && ($(idToHide).addClass("animated").addClass(hideAnimation), $(idToHide).one("webkitAnimationEnd oanimationend msAnimationEnd animationend", function() {
                $(this).addClass("hide"), idToShow && ("this" === idToShow && (idToShow = $(this).parent()), $(idToShow).addClass("animated").addClass(showAnimation))
            }))
        }), $(".js-toggle").click(function() {
            return idToHide = $(this).attr("data-id-hide"), idToShow = $(this).attr("data-id-show"), dataTimes = $(this).attr("data-times"), 0 > dataTimes ? !1 : (dataTimes > 0 && $(this).attr("data-times", "-1"), showAnimation = $(this).attr("data-animation-show"), hideAnimation = $(this).attr("data-animation-hide"), $(idToHide).addClass("animated").addClass(hideAnimation).removeClass(showAnimation), $(idToHide).one("webkitAnimationEnd oanimationend msAnimationEnd animationend", function() {
                $(this).addClass("hide"), $(idToShow).removeClass("hide").addClass("animated").addClass(showAnimation).removeClass(hideAnimation)
            }), $(this).attr("data-id-show", idToHide), void $(this).attr("data-id-hide", idToShow))
        }), $(".more").click(function(event) {
            return event.stopPropagation(), event.preventDefault(), curLimit = $(this).prev().attr("max"), curVal = $(this).prev().val(), curLimit == curVal ? !1 : (curVal = parseFloat(curVal), void $(this).prev().val(curVal + 1))
        }), $(".less").click(function(event) {
            return event.stopPropagation(), event.preventDefault(), curLimit = $(this).next().attr("min"), curVal = $(this).next().val(), curLimit == curVal ? !1 : (curVal = parseFloat(curVal), void $(this).next().val(curVal - 1))
        }), $("select.js-accordion-select").change(function() {
            optionShow = $("option:selected", this).attr("data-id-show"), $(optionShow).show(), optionHide = $("option:selected", this).attr("data-id-hide"), $(optionHide).hide(), optionSelected = $("option:selected", this).attr("data-id-trigger"), $('a[href="' + optionSelected + '"]').trigger("click")
        }), navigator.userAgent.match(/(iPhone|iPod|iPad)/i) && ($(window).scroll(function() {
            $currentScrollPos = $(document).scrollTop()
        }), $('[data-toggle="modal"]').click(function() {
            modalTarget = $(this).attr("data-target"), $(modalTarget).on("shown.bs.modal", function() {
                $("body").css({
                    position: "fixed"
                }), localStorage.cachedScrollPos = $currentScrollPos
            }), $(modalTarget).on("hidden.bs.modal", function() {
                $("body").css({
                    position: "relative"
                }), $("body").scrollTop(localStorage.cachedScrollPos)
            })
        })), $('input[type="month"], input[type="date"]').each(function() {
            datePlaceHolder = $(this).attr("placeholder"), $(this).before('<span class="js-fake-placeholder">' + datePlaceHolder + "</span>")
        }), $('input[type="month"], input[type="date"]').focus(function() {
            $(this).prev(".js-fake-placeholder").hide()
        }), $("textarea.js-autosize").keyup(function() {
            textareaResize = $(this), setTimeout(function() {
                textareaResize.removeAttr("style"), textareaHeight = textareaResize.get(0).scrollHeight, textareaResize.height(textareaHeight)
            }, 200)
        }), $(".js-upload-file").on("click", function() {
            var inputfile = "#" + $(this).attr("for"),
                filenameContainer = $(this).attr("data-file-name-holder");
            filenameContainer && $(inputfile).change(function() {
                for (var content = "<ul>", i = 0; i < $(this).get(0).files.length; ++i) content += "<li>" + $(this).get(0).files[i].name + "</li>";
                content += "</ul>", $(filenameContainer).html(content)
            })
        }), $(window).on("resize", function() {})
    }), $(function() {
        var anchoVentana = $(window).width();
        767 > anchoVentana && $("#header").affix({
            offset: {
                top: 0,
                bottom: function() {}
            }
        }), $(".menu-canvas a + .caret").on("click", function() {
            $(this).next().toggleClass("in"), $(this).toggleClass("opened")
        }), $("header a#search").mouseenter(function() {
            $(this).parent().prev().removeClass("hidden")
        }), $("header .input-group").mouseleave(function() {}), $("html").click(function() {}), $("header .input-group").click(function() {}), $("#off-canvas-right a#search-canvas").click(function() {
            $(this).parent().prev().toggleClass("hidden")
        }), $(".has-submenu").mouseenter(function() {
            $(this).next().next().addClass("in"), $(this).parent().find(".caret").addClass("opened")
        }), $(".has-submenu + .padding-caret + .collapse").mouseleave(function() {
            setTimeout(function() {
                $(".caret.opened").removeClass("opened"), $(".collapse.in").removeClass("in")
            }, 5e3)
        }), $("header .main-nav .dropdown-menu").mouseleave(function() {
            setTimeout(function() {
                $(".main-nav li.open a.js-trigger").trigger("click")
            }, 1e4)
        }), $(".main-nav li a.js-trigger").mouseenter(function() {
            $(this).trigger("click")
        }), $(".dropdown-menu .padding-caret").click(function() {
            $(this).next().removeClass("in"), $(this).parent().parent().parent().addClass("opened"), $(this).find(".caret").removeClass("opened")
        }), initAsync(), $(window).on("resize", function() {})
    })
}({}, function() {
    return this
}());