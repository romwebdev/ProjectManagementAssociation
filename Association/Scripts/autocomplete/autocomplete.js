//domReady
$(function () {
    initAutoComplete();
});

//function init appelée dans le domReady
function initAutoComplete() {

    $("input[data-parents-autocomplete]").each(createAutocompleteParent);//input autocompleteparent
    clearInputParent();//clear input parent
    $(".modal-dialog").css("width", "850px");
    displayInputToAdult();// champs texte infos adulte

};

//fonction pour input autocomplete parent
function createAutocompleteParent() {
    var $input = $(this);
    if ($input.val() == null) {
        $input.prev().val("");
    }
    var options = {
        minLength: 3,
        source: function (request, response) {
            $.ajax({
                url: $input.attr("data-parents-autocomplete"),
                dataType: "Json",
                data: {
                    SearchParents: $input.val()
                },
                success: function (data) {
                    if (data != null && data != "") {
                        response($.map(data, function (item) {
                            return {
                                label: item.value,
                                value: item.value,
                                id: item.id,
                            };
                        }));
                    }
                    else {
                        //alert("Null");
                        response([{ label: 'No results found.', value: 'no results found', id: '' }]);
                    }
                },
                error: function (xhr, status, error) {
                    alert(xhr);
                }
            });
        },
        //commenté pour bug chrome
        //focus: function (event, ui) {
        //    // prevent autocomplete from updating the textbox
        //    //event.preventDefault();
        //    // manually update the textbox
        //    if (ui.item.id != '' && ui.item.id != null) {

        //        populateInputParent($input, ui);
        //    }
        //    else {
        //        return false;
        //    }
        //},
        select: function (event, ui) {
            // prevent autocomplete from updating the textbox
            //event.preventDefault();
            // manually update the textbox and hidden field
            if (ui.item.id != '' && ui.item.id != null) {

                populateInputParent($input, ui);
            }
            else {
                return false;
            }
        },
    };
    $input.autocomplete(options, "appendTo", "#myModal");
};
// fonction pour afficher ou non les input text adultes
function displayInputToAdult() {

    var radio = $('input[name="student_type"]:checked').val();
    if (radio == "True") {
        $('.studentAdress').hide();
    };

    $('input[type="radio"]').on("click", function () {
        var radio = $('input[name="student_type"]:checked').val();
        if (radio == "True") {
            $('.studentAdress').hide();

        }
        else {
            $('.studentAdress').show();

        }
    });
};

//fonction pour vider le champ parent sur close bouton
function clearInputParent() {
    $(document).on('click', '.clear-text', function () {
        //alert("click");
        //var $inputPrev = $(this).prev();
        var $parent = $(this).parent().prev();
        var $inputHidden = $parent.find('input[type="hidden"]');
        var $inputText = $inputHidden.next();

        $inputHidden.val("");
        $inputText.val("");
        $inputText.prop('disabled', false);
        //$inputPrev.val("");
        //$inputPrev.prev().val("");
        //$inputPrev.prop('disabled', false);
        $(this).addClass("hide");
    });
};

//fonction pour injecter parent dans input text et input hidden
function populateInputParent(input, ui) {
    var $input = input;
    var $parent = input.parent().next();
    $input.val(ui.item.label);
    $input.prev().val(ui.item.id);
    //$input.next().addClass("ui-icon");
    $parent.find('span').removeClass("hide");
    $input.prop('disabled', true);
    return false;

};