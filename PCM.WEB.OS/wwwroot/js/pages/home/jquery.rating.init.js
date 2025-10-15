
$(function () {

    function ratingEnable() {
      
        $('.example-css').barrating({
            theme: 'fontawesome-stars',
            showSelectedRating: false
        });
    }
 
    ratingEnable();

});

function goToPage(page, uniqueId) {

    window.location.href = page + "?uniqueId=" + uniqueId;
}
