
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

$(document).ready(function () {

    $("#phoneNumber").mask("(99) 99999-9999");

    var table = $('#tableMain').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        fixedColumns: {
            start: 0,
            end: 1
        },
        searching: true,
        lengthChange: false,
        pageLength: 15,
        processing: true,
        scrollX: true,
        scrollCollapse: true,
        serverSide: false,
        ajax: {
            url: "loadBranchUser",
            type: "POST",
            datatype: "json",
            data: {
                "username": ""
            },
            dataSrc: ""
        },
        rowId: 'partnerId',
        columns: [
            { data: "partnerId", orderable: false },
            { data: "partnerId" },
            { data: "partnerName" }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[1, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [0, 1] },
            { width: '20px', targets: [0] },
            { width: '150px', targets: [1] },
            {
                createdCell: function (td, cellData, rowData, row, col) {
                    if (col == 0) {
                        $(td).html("<label class='css-control css-control-success css-checkbox'><input type='checkbox' id='checkBranch' name='checkBranch' " + rowData.selectedBranch + " class='css-control-input'><span class='css-control-indicator'></span></label >");
                    }
                }, targets: [0]
            }
        ],
    });

});

$('#form').validate({
    ignore: [],
    errorClass: 'invalid-feedback animated fadeInDown',
    errorElement: 'div',
    onkeyup: false,
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
        'domain': {
            required: true
        },
        'username': {
            required: true
        },
        'wwid': {
            required: true
        },
        'name': {
            required: true
        },
        'email': {
            required: true,
            email: true
        },
        'profile': {
            required: true
        },
        'active': {
            required: true
        }
    },
    messages: {
        'domain': {
            required: messages.requiredField
        },
        'username': {
            required: messages.requiredField
        },
        'wwid': {
            required: messages.requiredField
        },
        'name': {
            required: messages.requiredField
        },
        'email': {
            required: messages.requiredField,
            email: messages.validEmail
        },
        'profile': {
            required: messages.requiredField
        },
        'active': {
            required: messages.requiredField
        }
    },
    submitHandler: function (form) {

        $("#btnSave").prop('disabled', true);

        var selectedBranches = [];

        $('#tableMain tbody tr').each(function () {
            var row = $(this);
            var checkbox = row.find('td:nth-child(1) input[type="checkbox"]');

            if (checkbox.is(':checked')) {
                var partnerId = row.attr('id');

                var newRecord = {
                    "partnerId": partnerId
                };

                // Adicionar o novo registro ao JSON
                selectedBranches.push(newRecord);
            }
        });

        // Valida se pelo menos uma branch foi selecionada
        if (selectedBranches.length === 0) {
            Swal.fire({
                title: messages.requiredSelectedRow,
                icon: "info",
                showDenyButton: false,
                showCancelButton: false,
            }).then((result) => {
                if (result) {
                    $("#btnSave").prop('disabled', false);
                    return false;
                }
            });
        } else {

            // Converte a lista em JSON e armazena no campo hidden
            $("#branchList").val(JSON.stringify(selectedBranches));

            return true;
        }

    },
    invalidHandler: function (e, validation) {
        $("#btnSave").prop('disabled', false);
    }
});