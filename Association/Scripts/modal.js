//$(function () {

//    $.ajaxSetup({ cache: false });

//    //$("a[data-modal]").on("click", function (e) {
//        $(".modal-popup").on("click", function (e) {
//        // hide dropdown if any
//       $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');


//        $('#myModalContent').load(this.href, function () {


//            $('#myModal').modal({
//                /*backdrop: 'static',*/
//                keyboard: true
//            }, 'show');

//            bindForm(this);
//        });

//        return false;
//    });


//});

//function bindForm(dialog) {

//    $('form', dialog).submit(function () {
//        if ($("input[type='file'") != null) { alert("Input file not null");}
//        var formData = new FormData(this);
//        $.ajax({
//            url: this.action,
//            type: this.method,
//            //contentType: false,
//            //data: formData,
//            mimeType: "multipart/form-data",
//            data: $(this).serialize(),
//            //data: formData,
//            success: function (result) {
//                if (result.success) {
//                    $('#myModal').modal('hide');
//                    //Refresh
//                    location.reload();
//                } else {
//                    $('#myModalContent').html(result);
//                    bindForm();
//                }
//            },
//            failure: function (data) {
//                alert(data);
//            }
//        });
//        return false;
//    });
//}