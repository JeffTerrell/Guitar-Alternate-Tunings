(function($) {

$(".drop-down-button").on("click", () => {
  $(".drop-down-div").slideUp();
  $(".drop-down-show").slideDown();
});

$(".no-show-button").on("click", () => {
  $(".drop-down-show").slideUp();
  $(".drop-down-div").slideDown();
});

})(jQuery);