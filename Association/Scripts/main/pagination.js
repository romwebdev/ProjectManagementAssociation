//Dom ready
$(function () {
    $('body').on('click', '#listPerson .pagination a', function (event) {
        event.preventDefault();
        console.log('page');
        //var searchString = $('#SearchString').val();
        //if (searchString == undefined || searchString == '') {
        //    searchString = '';
        //} else {
        //    searchString = '&searchString=' + searchString;
        //}
        var url = $(this).attr('href'); // + searchString;
        console.log(url);
        $.ajax({
            url: url,
            success: function (result) {
                ChangeUrl('index', url);
                $('#listPerson').html(result);
                initMain(); //Fonction pour appelé les fonctionnalités (modal, bindform, ...)
            }
        });
    });
});

//Change URL PAGE
function ChangeUrl(page, url) {
    if (typeof (history.pushState) != "undefined") {
        var obj = { Page: page, Url: url };
        history.pushState(obj, obj.Page, obj.Url);
    } else {
        alert("Browser does not support HTML5.");
    }
};