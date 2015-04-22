//$(document).ready(function () {

$(function () {
    initMain();
    //fonctionnalité pour reset le formulaire
    //appel de la fonction refresh ajax
    resetFormSearch();

});

function initMain() {

    $.ajaxSetup({ cache: false });

    //popup modal pour afficher les infos pour la création ou édition élève
    modal();

    //evènement pour cacher le modal lorsque une autre modal s'affiche
    $('#myModalParent').on('hidden.bs.modal', function (e) {
        e.preventDefault();
        $('#myModal').modal({
            backdrop: 'static',
            keyboard: true
        }, 'show');
        return false;
    })

    //fonctionnalité pour l'autocomplete de recherche
    $("input[data-otf-autocomplete]").each(createAutocomplete);
}

/*****************************************************************************
*********************************** Fonctions ********************************
*****************************************************************************/

function resetFormSearch() {
    $("#resetSearch").on("click", function (event) {
        //e.preventDefault();
        $(".ui-autocomplete-input").val("").delay("200");
        refresh();
    });
}
//fonction pour la recherche autocomplete
function createAutocomplete() {
    var $input = $(this);

    var options = {
        minLength: 3,
        select: submitAutoCompleteForm,
        source: function (request, response) {
            $.ajax({
                url: $input.attr("data-otf-autocomplete"),
                dataType: "Json",
                data: {
                    SearchString: $input.val() //valeur à rechercher
                },
                success: function (data) {
                    if (data != null && data != "") {
                        response($.map(data, function (item) {
                            return {
                                label: item.value,
                                value: item.label,
                            };
                        }));
                    }
                    else {
                        response([{ label: 'No results found.', value: null, id: '' }]);
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        },

    };


    $input.autocomplete(options);
};

//Envoie la valeur sélectionnée lors de l'autocomplete
function submitAutoCompleteForm(event, ui) {

    if (ui.item.value != null && ui.item.value != "" && ui.item.id != '') {
        var $input = $(this);
        $input.val(ui.item.value);
        var $form = $("#formSearch");
        var option = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
        };
        $.ajax(option).done(function (data) {
            var $target = $($form.attr("data-otf-target"));
            $target.replaceWith(data);
            initMain();

        });
        return false;
    }
};

//Fonction initialization to form 
function initializationForm() {
    inputFileStyle();
    datePickerForm();
    imagePreview();
    deleteImage();
};

//Fonctionnalité pour soumettre le formulaire Ajax
function bindForm(dialog, id) {
    $('form', dialog).submit(function (event) {
        event.preventDefault();

        var $form = $(this);
        var formdata = (window.FormData) ? new FormData($form[0]) : null;
        var data = (formdata !== null) ? formdata : $form.serialize();

        var option = {
            url: $form.attr('action'),
            type: $form.attr('method'),
            cache: false,
            contentType: false, // obligatoire pour de l'upload
            processData: false, // obligatoire pour de l'upload
            data: data,
            success: function (result) {
                if (result.success) {
                    //hide modal ouverte
                    $('#myModal' + id).modal('hide');

                    //id paramettre pour modal parent (multiple modal)
                    if (id != '') {

                        var nameParent = result.parentName;
                        var idParent = result.parentId;
                        $('#myModal').one('shown.bs.modal', function (a) {
                            $('#student_mobile').focus();
                            a.preventDefault();
                            //recherche d'un input non null et l'auto remplir
                            $('.studentParents input:text:visible').each(function () {
                                if ($(this).val().length === 0) {
                                    $(this).val(nameParent);
                                    $(this).prev().val(idParent);
                                    $(this).prop('disabled', true);
                                    var $parent = $(this).parent().next();
                                    $parent.find('span').removeClass("hide");
                                    //console.log("nom parent :" + nameParent)
                                    return false;
                                }
                            });
                            //message pour ajout d'un parent
                            //$('#msg').html("<div class='alert alert-success'>Le parent : " + nameParent + " a été ajouté</div>").fadeIn('slow'); //also show a success message
                            //$('#msg').delay(5000).fadeOut('slow');
                            msgAlert('#myModalContent #msg', "<div class='alert alert-success'>Le parent : " + nameParent + " a été ajouté</div>", true);
                        });


                    }
                    else {
                        refresh();
                    }


                }
                else if (result.error) {
                    //$('#myModalContent' + id + ' #msg').html("<div class='alert alert-danger'>L'entité <b style='text-decoration: underline;'>" + result.name + "</b> existe déja en base !!! </div>").fadeIn('slow');
                    msgAlert('#myModalContent' + id + ' #msg', "<div class='alert alert-danger'>L'entité <b style='text-decoration: underline;'>" + result.name + "</b> existe déja en base !!! </div>");

                }
                else {
                    $('#myModalContent' + id).html(result);
                    //fonction regroupant divers fonctions pour le form
                    initializationForm();
                    bindForm(dialog, id);
                    
                }
            },
            error: function (data) {
                alert(data);
            }
        };
        $.ajax(option).done();
        return false;
    });
};

//Fonction pour afficher un message lors d'une requête
function msgAlert(el, msg, success) {

    $(el).html(msg).fadeIn('slow');
    if (success) {
        $(el).delay(3000).fadeOut(2000, "linear");
    }


};
//fonction de rafraichissement
//appel : 
//      - bindform() 
//      - resetFormSearch()
function refresh() {

    var option = {
        url: VarsApp.actionIndex, //variable definie dans layout
        type: 'GET',
        success: function (result) {
            msgAlert('.body-content #msg', "<div class='alert alert-success'>Le contenu a été mis à jour</div>", true);
        }
    };
    $.ajax(option).done(function (result) {
       
        //$('#listPerson').replaceWith(result);
        $('#listPerson').html(result);
        initMain();
        //modal();
    });

    return false;
};

//fonction modal
function modal() {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        e.preventDefault();
        //chargement de la div du contenu de l'élève
        $('#myModalContent').load(this.href, function () {
            initializationForm();
            $('#myModal').modal({
                backdrop: 'static', // modal static (clic desactivé en dehors du modal)
                keyboard: true
            }, 'show');
            //fonction pour envoyer le formulaire 
            bindForm(this, id = ''); // id parametre pour modal parent
            modalParent();
        });


        return false;
    });


};

//fonction modal parent lors de la création ou édition d'un élève
function modalParent() {
    $.ajaxSetup({ cache: false });
    $('.parents').on("click", function (e) {
        //Hide le modal student mode edit
        $('#myModal').modal('hide');
        //chargement de la div creation parent
        $('#myModalContentParent').load(this.href, function () {
            //initializationForm();
            $('#myModalParent').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            //fonction pour envoyer le formulaire 
            bindForm(this, id = 'Parent');
        });

        return false;
    });


};

//Fonction pour input type file Design 
function inputFileStyle() {
    $(":file").filestyle({
        icon: false,
        buttonText: "Fichier",
    });
}

//Fonction datepicker 
function datePickerForm() {
    //var datepicker = $.fn.datepicker.noConflict(); // return $.fn.datepicker to previously assigned value
    //$.fn.bootstrapDP = datepicker;
    //$.fn.datepicker.defaults.format = "dd/mm/yyyy";
    var FromEndDate = new Date();
    $(".form_datetime").datepicker(
        {
            format: "dd/mm/yyyy",
            clearBtn: true,
            todayBtn: true,
            autoclose: true,
            orientation: 'top',
            //endDate: FromEndDate,
            startView: 2,
            todayHighlight: true,
            language: 'fr'
        }
    );
};

//Fonction Image Preview 
function imagePreview() {
    // A chaque sélection de fichier
    $('form').find('input[name="image"]').on('change', function (e) {
        var files = $(this)[0].files;

        if (files.length > 0) {
            // On part du principe qu'il n'y qu'un seul fichier
            // étant donné que l'on a pas renseigné l'attribut "multiple"
            var file = files[0],
                $image_preview = $('#image_preview');

            // Ici on injecte les informations recoltées sur le fichier pour l'utilisateur
            $image_preview.find('.thumbnail').removeClass('hidden');
            $image_preview.find('img').attr('src', window.URL.createObjectURL(file));
            $image_preview.find('img').attr('title', file.name);
            //$image_preview.find('h4').html(file.name);
            //$image_preview.find('.caption p:first').html(file.size + ' bytes');
        }
    });

    // Bouton "Annuler" pour vider le champ d'upload
    $('#image_preview').find('button[type="button"]').on('click', function (e) {
        e.preventDefault();

        $('#my_form').find('input[name="image"]').val('');
        $('#image_preview').find('.thumbnail').addClass('hidden');
    });
};

//Fonction Image delete 
function deleteImage() {
    $(".deleteImage").click(function (event) {
        event.preventDefault();
        //var formData = new FormData(this);
        var $image = $(this);

        var option = {
            url: $image.attr('href'),
            type: 'POST',
            //data: formData,
            contentType: false, // obligatoire pour de l'upload
            processData: false, // obligatoire pour de l'upload
            data: $image.attr('id'),
            success: function (result) {
                if (result.success) {
                    $('.thumbnail ').addClass("hidden");
                    $('.image-class').attr('src', null);
                    //$('#image').attr("data-disable", "False");
                    inputFileStyle(false);
                    $('.bootstrap-filestyle').find("input").val("");
                } else {

                }
            },
            error: function (data) {
                alert(data);
            }
        };
        $.ajax(option).done();
        return false;
    });
};