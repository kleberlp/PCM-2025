
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
var table;

var jsonData = [];

$(document).ready(function () {

    document.querySelector("#frequency").disabled = true;

    $('#btnSaveAlert').on('click', function () {

        $("#formAlert").submit();

    });

    $('#btnSave').on('click', function () {

        $("#form").submit();

    });

    $('#recurring').on('change', function () {

        if (document.querySelector("#recurring").value == 1) {
            document.querySelector("#frequency").disabled = false;
        } else {
            document.querySelector("#frequency").disabled = true;
        }

    });

    table = $('#tableMain').DataTable({
        fixedColumns: {
            start: 0,
            end: 1
        },
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollX: true,
        scrollCollapse: true,
        serverSide: false,
        data: jsonData,
        dom: "<'row'<'col-sm-12 col-md-6'B><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            {
                extend: 'copy',
                text: "<i class='fas fa-copy text-primary'></i>",
                titleAttr: messages.clickToCopy,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2'
            },
            {
                extend: 'excel',
                text: "<i class='fas fa-file-excel text-success'></i>",
                titleAttr: messages.clickToExcel,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2'
            },
            {
                extend: 'pdf',
                text: "<i class='fas fa-file-pdf text-danger'></i>",
                titleAttr: messages.clickToPdf,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2'
            },
            {
                extend: 'colvis',
                text: "<i class='fas fa-columns text-black'></i>",
                titleAttr: messages.clickToConfig,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2',
                columns: [0, 1, 2, 3]
            }
        ],
        columns: [
            { data: "alertType" },
            { data: "deliveryType" },
            { data: "recurring" },
            { data: "frequency" },
            {
                orderable: false,
                data: null,
                defaultContent: "<div class='btn-group'> " +
                    "<button type='button' class='btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle tippy-btn' title='" + messages.clickToDelete + "' data-tippy-arrow='true' data-tippy-arrowTransform='scale(0.75)' data-tippy-animation='fade' id='btnDelete' data-bstoggle='dropdown' aria-expanded='false'><i class='fas fa-trash-alt'></i></button></div> "
            }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[0, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [1, 2, 3, 4] },
            { width: '20px', targets: [4] },
            { width: '100px', targets: [1, 2] },
            { width: '150px', targets: [3] }
        ],
    });


    $('#tableMain tbody').on('click', 'button', function () {

        var data = table.row($(this).closest('tr')).data();

        if (this.id == "btnDelete") {
            deleteRegister(data);
        }

    });

    function deleteRegister(data) {

        swal.fire({
            title: messages.msgQuestionDelete,
            text: messages.msgNotPossibleReverse,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: messages.yes,
            cancelButtonText: messages.no,
            reverseButtons: true
        }).then(function (result) {
            if (result.value) {

                jsonData = jsonData.filter(function (item) {
                    return item.alertTypeId !== data.alertTypeId || item.deliveryTypeId !== data.deliveryTypeId // Exclui o item com o id passado
                });

                $("#details").val(JSON.stringify(jsonData));


                // Atualizar a tabela DataTable com o novo registro
                table.clear().rows.add(jsonData).draw();

            }
        });
    }

});

$.validator.addMethod("validPK", function (value, element) {

    var result = false;

    jQuery.ajax({
        method: "POST",
        url: "isValidNotification",
        async: false,
        data: {
            "branch": $("#branch").val(),
            "description": value
        },
        dataType: "json",
        success: function (response) {
            result = response;
        }
    });

    return result;

}, messages.validPK);

$.validator.addMethod("validPKAlert", function (value, element) {

    var pkExists = jsonData.some(function (item) {
        return item.alertTypeId === value && item.deliveryTypeId === $("#deliveryType").val();  // Compara os nomes de forma case-insensitive
    });

    return !pkExists;

}, messages.validPK);

$.validator.addMethod("validFrequency", function (value, element) {

    if (value == "" && document.querySelector("#frequency").disabled == false) {
        return false
    } else {
        return true
    }

}, messages.requiredField);

$('#form').validate({
    ignore: [],
    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',
    onkeyup: true,
    onclick: false,
    onfocusout: false,
    errorPlacement: function (error, e) {
        jQuery(e).parents('.form-group > div > div').append(error);
    },
    highlight: function (e) {
        jQuery(e).closest('.form-group > div > div').removeClass('is-invalid').addClass('is-invalid');
    },
    success: function (e) {
        jQuery(e).closest('.form-group > div > div').removeClass('is-invalid');
        jQuery(e).remove();
    },
    rules: {
        'branch': {
            required: true
        },
        'description': {
            required: true,
            validPK: true
        },
        'active': {
            required: true
        }
    },
    messages: {
        'branch': {
            required: messages.requiredField
        },
        'description': {
            required: messages.requiredField,
            validPK: messages.validPK
        },
        'active': {
            required: messages.requiredField
        }
    },
    submitHandler: function (form) {
        return true;
    },
    invalidHandler: function (e, validation) {
        $("#btnSave").prop('disabled', false);
    }
});

$('#formAlert').validate({
    ignore: [],
    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',
    onkeyup: true,
    onclick: false,
    onfocusout: false,
    errorPlacement: function (error, e) {
        jQuery(e).parents('.form-group > div > div').append(error);
    },
    highlight: function (e) {
        jQuery(e).closest('.form-group > div > div').removeClass('is-invalid').addClass('is-invalid');
    },
    success: function (e) {
        jQuery(e).closest('.form-group > div > div').removeClass('is-invalid');
        jQuery(e).remove();
    },
    rules: {
        'alertType': {
            required: true,
            validPKAlert: true
        },
        'recurring': {
            required: true
        },
        'frequency': {
            validFrequency: true
        }
    },
    messages: {
        'alertType': {
            required: messages.requiredField,
            validPKAlert: messages.validPK
        },
        'recurring': {
            required: messages.requiredField
        },
        'frequency': {
            validFrequency: messages.requiredField
        }
    },
    submitHandler: function (form) {
        $("#btnSaveAlert").prop('disabled', true);

        var newRecord = {
            "alertType": $('#alertType option:selected').text(),
            "recurring": $('#recurring option:selected').text(), 
            "frequency": document.querySelector("#frequency").value,
            "deliveryType": $('#deliveryType option:selected').text(),
            "deliveryTypeId": document.querySelector("#deliveryType").value,
            "alertTypeId": document.querySelector("#alertType").value,
            "recurringValue": document.querySelector("#recurring").value
        };

        // Adicionar o novo registro ao JSON
        jsonData.push(newRecord);

        // Atualizar a tabela DataTable com o novo registro
        table.clear().rows.add(jsonData).draw();

        $("#details").val(JSON.stringify(jsonData));

        clearAlert();

        $("#btnSaveAlert").prop('disabled', false);
    },
    invalidHandler: function (e, validation) {
        $("#btnSaveAlert").prop('disabled', false);
    }
});

function clearAlert() {

    document.querySelector("#alertType").value = "";
    document.querySelector("#recurring").value = "";
    document.querySelector("#frequency").value = "";
    document.querySelector("#deliveryType").value = "";
}