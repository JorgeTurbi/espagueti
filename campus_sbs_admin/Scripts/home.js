$(function () {
    var jssor_SlideoTransitions = [
      /*[{ b: 0, d: 200, x: 50, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 600, x: 50, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 1000, x: 50, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 1400, x: 50, o: 1, e: { y: 0 } }],*/

      [{ b: 0, d: 200, y: 40, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 600, y: 160, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 1000, y: 290, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 1400, y: 400, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 200, x: 850, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 600, x: 850, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 1000, x: 850, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 1400, x: 850, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 200, y: 40, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 600, y: 160, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 1000, y: 290, o: 1, e: { y: 0 } }],
      [{ b: 0, d: 1400, y: 400, o: 1, e: { y: 0 } }]
    ];
    var jssor_options = {
        $AutoPlay: 1,
        $LazyLoading: 1,
        $CaptionSliderOptions: {
            $Class: $JssorCaptionSlideo$,
            $Transitions: jssor_SlideoTransitions
        },
        $ArrowNavigatorOptions: {
            $Class: $JssorArrowNavigator$
        },
        $BulletNavigatorOptions: {
            $Class: $JssorBulletNavigator$,
            $SpacingX: 20,
            $SpacingY: 20
        }
    };
    var jssor_slider = new $JssorSlider$("jssor-home", jssor_options);

    /*#region responsive code begin*/
    var MAX_WIDTH = 3000;

    function ScaleSlider() {
        var containerElement = jssor_slider.$Elmt.parentNode;
        var containerWidth = containerElement.clientWidth;

        if (containerWidth) {
            var expectedWidth = Math.min(MAX_WIDTH || containerWidth, containerWidth);
            jssor_slider.$ScaleWidth(expectedWidth);
        }
        else
            window.setTimeout(ScaleSlider, 30);
    }

    ScaleSlider();

    $Jssor$.$AddEvent(window, "load", ScaleSlider);
    $Jssor$.$AddEvent(window, "resize", ScaleSlider);
    $Jssor$.$AddEvent(window, "orientationchange", ScaleSlider);
    /*#endregion responsive code end*/
});
