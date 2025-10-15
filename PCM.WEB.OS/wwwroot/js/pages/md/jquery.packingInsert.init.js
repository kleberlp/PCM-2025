
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
var table;

var jsonData = [];

$(document).ready(function () {

    $(".select2").select2({ width: '100%' });
    
    $('#btnSaveDetail').on('click', function () {

        $("#formDetail").submit();

    });

    $('#btnSave').on('click', function () {

        $("#form").submit();

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
                columns: [0, 1, 2, 3, 4]
            }
        ],
        columns: [
            { data: "classification" },
            { data: "family" },
            { data: "capacity" },
            { data: "startNumber" },
            { data: "endNumber" },
            {
                orderable: false,
                data: null,
                defaultContent: "<div class='btn-group'> " +
                    "<button type='button' class='btn btn-sm btn-outline-secondary waves-light waves-effect dropdown-toggle tippy-btn' title='" + messages.clickToDelete + "' data-tippy-arrow='true' data-tippy-arrowTransform='scale(0.75)' data-tippy-animation='fade' id='btnDelete' data-bstoggle='dropdown' aria-expanded='false'><i class='fas fa-trash-alt'></i></button>" +
                    "</div> "
            }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[1, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [0, 2, 3, 4, 5] },
            { width: '20px', targets: [5] },
            { width: '150px', targets: [0, 2, 3, 4] }
        ]
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
                    return item.classificationId !== data.classificationId || item.familyId !== data.familyId
                });

                table.clear().rows.add(jsonData).draw();

                clearPackingDetails();

            }
        });
    }

});

$.validator.addMethod("validPK", function (value, element) {

    var result = false;

    jQuery.ajax({
        method: "POST",
        url: "isValidPacking",
        async: false,
        data: {
            "code": $("#code").val()
        },
        dataType: "json",
        success: function (response) {
            result = response;
        }
    });

    return result;

}, messages.validPK);

$('#form').validate({
    ignore: [],
    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',
    onkeyup: false,
    onclick: false,
    onfocusout: false,
    errorPlacement: function (error, e) {
        jQuery(e).parents('.accordion-body > div > div').append(error);
    },
    highlight: function (e) {
        jQuery(e).closest('.accordion-body > div > div').removeClass('is-invalid').addClass('is-invalid');
    },
    success: function (e) {
        jQuery(e).closest('.accordion-body > div > div').removeClass('is-invalid');
        jQuery(e).remove();
    },
    rules: {
        'code': {
            required: true,
            validPK: true
        },
        'description': {
            required: true
        },
        'active': {
            required: true
        }
    },
    messages: {
        'code': {
            required: messages.requiredField,
            validPK: messages.validPK
        },
        'description': {
            required: messages.requiredField
        },
        'active': {
            required: messages.requiredField
        }
    },
    submitHandler: function (form) {

        $("#jsonDetails").val(JSON.stringify(jsonData));

        return true;
    },
    invalidHandler: function (e, validation) {
        $("#btnSave").prop('disabled', false);
    }
});

$.validator.addMethod("validPKTable", function (value, element) {

    var pkExists = jsonData.some(function (item) {
        return parseInt(item.classificationId, 10) === parseInt($("#classificationId").val(), 10) && parseInt(item.familyId, 10) === parseInt($("#familyId").val(), 10)
    });

    return !pkExists;

}, messages.validPK);

$('#formDetail').validate({
    ignore: [],
    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',
    onkeyup: false,
    onclick: false,
    onfocusout: false,
    errorPlacement: function (error, e) {
        jQuery(e).parents('.accordion-body > div > div').append(error);
    },
    highlight: function (e) {
        jQuery(e).closest('.accordion-body > div > div').removeClass('is-invalid').addClass('is-invalid');
    },
    success: function (e) {
        jQuery(e).closest('.accordion-body > div > div').removeClass('is-invalid');
        jQuery(e).remove();
    },
    rules: {
        'classificationId': {
            required: true
        },
        'familyId': {
            required: true,
            validPKTable: true
        },
        'capacity': {
            required: true
        },
        'startNumber': {
            required: true
        }
    },
    messages: {
        'classificationId': {
            required: messages.requiredField
        },
        'familyId': {
            required: messages.requiredField,
            validPKTable: messages.validPK
        },
        'capacity': {
            required: messages.requiredField
        },
        'startNumber': {
            required: messages.requiredField
        }
    },
    submitHandler: function (form) {

        $("#btnSaveDetail").prop('disabled', true);

        var newRecord = {
            "classificationId": document.querySelector("#classificationId").value,
            "familyId": document.querySelector("#familyId").value,
            "capacity": document.querySelector("#capacity").value,
            "startNumber": document.querySelector("#startNumber").value,
            "endNumber": document.querySelector("#endNumber").value,
            "classification": document.querySelector("#classificationId option:checked").textContent,
            "family": document.querySelector("#familyId option:checked").textContent
        };

        jsonData.push(newRecord);

        table.clear().rows.add(jsonData).draw();

        clearPackingDetails();

        $("#btnSaveDetail").prop('disabled', false);
    },
    invalidHandler: function (e, validation) {
        $("#btnSaveDetail").prop('disabled', false);
    }
});

function clearPackingDetails() {

    $("#classificationId").val(null).trigger('change');
    $("#familyId").val(null).trigger('change');
    document.querySelector("#classificationId").value = "";
    document.querySelector("#familyId").value = "";
    document.querySelector("#capacity").value = "";
    document.querySelector("#startNumber").value = "";
    document.querySelector("#endNumber").value = "";

}
