(function ($) {
    $.Site.Libraries.Preloader =
    {
        Show: function () {
            $("#status").fadeIn();
            $("#preloader").delay(350).fadeIn("slow");
        },
        Hide: function () {
            $("#status").fadeOut();
            $("#preloader").delay(350).fadeOut("slow");
        }
    };
})(jQuery);