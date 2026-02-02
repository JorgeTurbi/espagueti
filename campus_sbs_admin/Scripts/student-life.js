$(function(){

    // Slider.
    $(document).ready(function ($) {
        var options = {
            $AutoPlay: 0,                                   //[Optional] Auto play or not, to enable slideshow, this option must be set to greater than 0. Default value is 0. 0: no auto play, 1: continuously, 2: stop at last slide, 4: stop on click, 8: stop on user navigation (by arrow/bullet/thumbnail/drag/arrow key navigation)
            $AutoPlaySteps: 1,                              //[Optional] Steps to go for each navigation request (this options applys only when slideshow disabled), the default value is 1
            $Idle: 2000,                                    //[Optional] Interval (in milliseconds) to go for next slide since the previous stopped if the slider is auto playing, default value is 3000
            $PauseOnHover: 1,                               //[Optional] Whether to pause when mouse over if a slider is auto playing, 0 no pause, 1 pause for desktop, 2 pause for touch device, 3 pause for desktop and touch device, 4 freeze for desktop, 8 freeze for touch device, 12 freeze for desktop and touch device, default value is 1

            $ArrowKeyNavigation: 1,                         //[Optional] Steps to go for each navigation request by pressing arrow key, default value is 1.
            $SlideEasing: $Jease$.$OutQuint,                //[Optional] Specifies easing for right to left animation, default value is $Jease$.$OutQuad
            $SlideDuration: 800,                            //[Optional] Specifies default duration (swipe) for slide in milliseconds, default value is 500
            $MinDragOffsetToSlide: 20,                      //[Optional] Minimum drag offset to trigger slide, default value is 20
            //$SlideWidth: 600,                             //[Optional] Width of every slide in pixels, default value is width of 'slides' container
            //$SlideHeight: 300,                            //[Optional] Height of every slide in pixels, default value is height of 'slides' container
            $SlideSpacing: 0,                               //[Optional] Space between each slide in pixels, default value is 0
            $UISearchMode: 1,                               //[Optional] The way (0 parellel, 1 recursive, default value is 1) to search UI components (slides container, loading screen, navigator container, arrow navigator container, thumbnail navigator container etc).
            $PlayOrientation: 1,                            //[Optional] Orientation to play slide (for auto play, navigation), 1 horizental, 2 vertical, 5 horizental reverse, 6 vertical reverse, default value is 1
            $DragOrientation: 1,                            //[Optional] Orientation to drag slide, 0 no drag, 1 horizental, 2 vertical, 3 either, default value is 1 (Note that the $DragOrientation should be the same as $PlayOrientation when $Cols is greater than 1, or parking position is not 0)

            $ArrowNavigatorOptions: {                       //[Optional] Options to specify and enable arrow navigator or not
                $Class: $JssorArrowNavigator$,              //[Requried] Class to create arrow navigator instance
                $ChanceToShow: 2,                           //[Required] 0 Never, 1 Mouse Over, 2 Always
                $Steps: 1                                   //[Optional] Steps to go for each navigation request, default value is 1
            },

            $BulletNavigatorOptions: {                      //[Optional] Options to specify and enable navigator or not
                $Class: $JssorBulletNavigator$,             //[Required] Class to create navigator instance
                $ChanceToShow: 2,                           //[Required] 0 Never, 1 Mouse Over, 2 Always
                $Steps: 1,                                  //[Optional] Steps to go for each navigation request, default value is 1
                $SpacingX: 12,                              //[Optional] Horizontal space between each item in pixel, default value is 0
                $Orientation: 1                             //[Optional] The orientation of the navigator, 1 horizontal, 2 vertical, default value is 1
            }
        };

        var jssor_slider1 = new $JssorSlider$("slider-student", options);

        // slider 1 responsive code begin
        function ScaleSlider1() {
            var parentWidth = jssor_slider1.$Elmt.parentNode.clientWidth;
            if (parentWidth) {
                jssor_slider1.$ScaleWidth(parentWidth);
            }
            else
                window.setTimeout(ScaleSlider1, 30);
        }
        ScaleSlider1();

        $(window).bind("load", ScaleSlider1);
        $(window).bind("resize", ScaleSlider1);
        $(window).bind("orientationchange", ScaleSlider1);
        // slider 1 responsive code end
    });
});