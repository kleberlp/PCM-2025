
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

var table;

$(document).ready(function () {

    table = $('#tableMain').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: true,
        fixedColumns: {
            start: 0,
            end: 6
        },
        lengthChange: false,
        pageLength: 15,
        paging: false,
        processing: true,
        scrollX: true,
        scrollCollapse: true,
        serverSide: false,
        "ajax": {
            url: "loadFormCarrier",
            type: "POST",
            datatype: "json",
            dataSrc: ""
        },
        "rowId": 'id',
        "buttons": [
            { extend: 'copy', text: "Copiar" },
            { extend: 'excel', text: "Excel" },
            { extend: 'pdf', text: "PDF" },
            { extend: 'colvis', text: "Config" }
        ],
        "columns": [
            { data: "groupDescription" },
            { data: "description" },
            { data: "roleView", orderable: false },
            { data: "roleInsert", orderable: false },
            { data: "roleUpdate", orderable: false },
            { data: "roleDelete", orderable: false },
            { data: "rolePrint", orderable: false },
            { data: "roleAdmin", orderable: false }
        ],
        "language": {
            "emptyTable": messages.emptyTable,
            "info": "",
            "infoEmpty": "",
            "infoFiltered": "",
        },
        order: [[0, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [2, 3, 4, 5, 6, 7] },
            { width: '80px', targets: [2, 3, 4, 5, 6, 7] },
            {
                createdCell: function (td, cellData, rowData, row, col) {
                    if (col == 2) {
                        if (rowData.allowView == true) {
                            $(td).html("<label class='css-control css-control-success css-checkbox'><input type='checkbox' id='checkView' name='checkView' class='css-control-input'" + (rowData.roleView ? "checked" : "") + "><span class='css-control-indicator'></span></label >");
                        } else {
                            $(td).html("");
                        }
                    } else if (col == 3) {
                        if (rowData.allowInsert == true) {
                            $(td).html("<label class='css-control css-control-success css-checkbox'><input type='checkbox' id='checkInsert' name='checkInsert' class='css-control-input'" + (rowData.roleInsert ? "checked" : "") + "><span class='css-control-indicator'></span></label >");
                        } else {
                            $(td).html("");
                        }
                    } else if (col == 4) {
                        if (rowData.allowUpdate == true) {
                            $(td).html("<label class='css-control css-control-success css-checkbox'><input type='checkbox' id='checkUpdate' class='css-control-input'" + (rowData.roleUpdate ? "checked" : "") + "><span class='css-control-indicator'></span></label >");
                        } else {
                            $(td).html("");
                        }
                    } else if (col == 5) {
                        if (rowData.allowDelete == true) {
                            $(td).html("<label class='css-control css-control-success css-checkbox'><input type='checkbox' id='checkDelete' class='css-control-input'" + (rowData.roleDelete ? "checked" : "") + "><span class='css-control-indicator'></span></label >");
                        } else {
                            $(td).html("");
                        }
                    } else if (col == 6) {
                        if (rowData.allowPrint == true) {
                            $(td).html("<label class='css-control css-control-success css-checkbox'><input type='checkbox' id='checkPrint' class='css-control-input'" + (rowData.rolePrint ? "checked" : "") + "><span class='css-control-indicator'></span></label >");
                        } else {
                            $(td).html("");
                        }
                    } else if (col == 7) {
                        if (rowData.allowAdmin == true) {
                            $(td).html("<label class='css-control css-control-success css-checkbox'><input type='checkbox' id='checkAdmin' class='css-control-input'" + (rowData.roleAdmin ? "checked" : "") + "><span class='css-control-indicator'></span></label >");
                        } else {
                            $(td).html("");
                        }
                    }

                }, targets: [2, 3, 4, 5, 6, 7]
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
        'description': {
            required: true
        },
        'active': {
            required: true
        }
    },
    messages: {
        'description': {
            required: messages.requiredField
        },
        'active': {
            required: messages.requiredField
        }
    },
    submitHandler: function (form) {

        $("#btnSave").prop('disabled', true);

        var table = $('#tableMain').DataTable();
        var data = [];

        $('#tableMain tbody tr').each(function () {
            var row = $(this);
            var id = row.data('id'); // Pega o ID da linha

            // Pega o estado dos checkboxes corretamente usando find() para acessar os checkboxes dentro de cada coluna.
            var roleView = row.find('td:nth-child(3) input[type="checkbox"]').is(':checked');
            var roleInsert = row.find('td:nth-child(4) input[type="checkbox"]').is(':checked');
            var roleUpdate = row.find('td:nth-child(5) input[type="checkbox"]').is(':checked');
            var roleDelete = row.find('td:nth-child(6) input[type="checkbox"]').is(':checked');
            var rolePrint = row.find('td:nth-child(7) input[type="checkbox"]').is(':checked');
            var roleAdmin = row.find('td:nth-child(8) input[type="checkbox"]').is(':checked');

            data.push({
                id: row.attr('id'),
                roleView: roleView,
                roleInsert: roleInsert,
                roleUpdate: roleUpdate,
                roleDelete: roleDelete,
                rolePrint: rolePrint,
                roleAdmin: roleAdmin
            });
        });

        $("#formRole").val(JSON.stringify(data));

        return true;

    },
    invalidHandler: function (e, validation) {
        $("#btnSave").prop('disabled', false);
    }
})