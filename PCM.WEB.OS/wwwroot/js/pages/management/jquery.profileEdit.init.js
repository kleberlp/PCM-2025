
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
        processing: true,
        scrollX: true,
        scrollCollapse: true,
        paging: false,
        serverSide: false,
        ajax: {
            url: "loadForm",
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.profileId = $("#profileId").val();
            },
            dataSrc: ""
        },
        rowId: 'id',

        // ------------- ATENÇÃO: Aqui ativamos o RowGroup -------------
        rowGroup: {
            dataSrc: 'groupDescription',
            startRender: function (rows, group) {
                // Retorna um <tr> com 8 <td>, um para cada coluna
                return $('<tr class="bg-soft-blue text-black" />').append(
                    $('<td/>').attr('colspan', 2).html('<strong>' + group + '</strong>'),
                    $('<td/>').addClass('text-center').html(`<input type="checkbox" class="check-group" data-group="${group}" data-perm="checkView">`),
                    $('<td/>').addClass('text-center').html(`<input type="checkbox" class="check-group" data-group="${group}" data-perm="checkInsert">`),
                    $('<td/>').addClass('text-center').html(`<input type="checkbox" class="check-group" data-group="${group}" data-perm="checkUpdate">`),
                    $('<td/>').addClass('text-center').html(`<input type="checkbox" class="check-group" data-group="${group}" data-perm="checkDelete">`),
                    $('<td/>').addClass('text-center').html(`<input type="checkbox" class="check-group" data-group="${group}" data-perm="checkPrint">`),
                    $('<td/>').addClass('text-center').html(`<input type="checkbox" class="check-group" data-group="${group}" data-perm="checkAdmin">`),
                );
            }
        },
        // -------------------------------------------------------------

        buttons: [
            { extend: 'copy', text: "Copiar" },
            { extend: 'excel', text: "Excel" },
            { extend: 'pdf', text: "PDF" },
            { extend: 'colvis', text: "Config" }
        ],
        columns: [
            { data: "groupDescription" },         // [0] -> Oculto ou visível, conforme sua necessidade
            { data: "description" },              // [1]
            { data: "roleView", orderable: false },   // [2]
            { data: "roleInsert", orderable: false }, // [3]
            { data: "roleUpdate", orderable: false }, // [4]
            { data: "roleDelete", orderable: false }, // [5]
            { data: "rolePrint", orderable: false },  // [6]
            { data: "roleAdmin", orderable: false }   // [7]
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[0, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [2, 3, 4, 5, 6, 7] },
            { width: '80px', targets: [2, 3, 4, 5, 6, 7] },
            {
                createdCell: function (td, cellData, rowData, row, col) {
                    // Ajusta o HTML de cada célula para exibir ou não o checkbox
                    if (col === 2) {
                        if (rowData.allowView) {
                            $(td).html(
                                `<label class='css-control css-control-success css-checkbox'>
                                <input type='checkbox' id='checkView' name='checkView' class='css-control-input' ${rowData.roleView ? "checked" : ""}>
                                <span class='css-control-indicator'></span>
                             </label>`
                            );
                        } else {
                            $(td).html("");
                        }
                    }
                    else if (col === 3) {
                        if (rowData.allowInsert) {
                            $(td).html(
                                `<label class='css-control css-control-success css-checkbox'>
                                <input type='checkbox' id='checkInsert' name='checkInsert' class='css-control-input' ${rowData.roleInsert ? "checked" : ""}>
                                <span class='css-control-indicator'></span>
                             </label>`
                            );
                        } else {
                            $(td).html("");
                        }
                    }
                    else if (col === 4) {
                        if (rowData.allowUpdate) {
                            $(td).html(
                                `<label class='css-control css-control-success css-checkbox'>
                                <input type='checkbox' id='checkUpdate' name='checkUpdate' class='css-control-input' ${rowData.roleUpdate ? "checked" : ""}>
                                <span class='css-control-indicator'></span>
                             </label>`
                            );
                        } else {
                            $(td).html("");
                        }
                    }
                    else if (col === 5) {
                        if (rowData.allowDelete) {
                            $(td).html(
                                `<label class='css-control css-control-success css-checkbox'>
                                <input type='checkbox' id='checkDelete' name='checkDelete' class='css-control-input' ${rowData.roleDelete ? "checked" : ""}>
                                <span class='css-control-indicator'></span>
                             </label>`
                            );
                        } else {
                            $(td).html("");
                        }
                    }
                    else if (col === 6) {
                        if (rowData.allowPrint) {
                            $(td).html(
                                `<label class='css-control css-control-success css-checkbox'>
                                <input type='checkbox' id='checkPrint' name='checkPrint' class='css-control-input' ${rowData.rolePrint ? "checked" : ""}>
                                <span class='css-control-indicator'></span>
                             </label>`
                            );
                        } else {
                            $(td).html("");
                        }
                    }
                    else if (col === 7) {
                        if (rowData.allowAdmin) {
                            $(td).html(
                                `<label class='css-control css-control-success css-checkbox'>
                                <input type='checkbox' id='checkAdmin' class='css-control-input' ${rowData.roleAdmin ? "checked" : ""}>
                                <span class='css-control-indicator'></span>
                             </label>`
                            );
                        } else {
                            $(td).html("");
                        }
                    }
                },
                targets: [2, 3, 4, 5, 6, 7]
            }
        ],
    });

});

$(document).on('change', '.check-group', function () {
    var group = $(this).data('group');      // Valor do grupo (groupDescription)
    var perm = $(this).data('perm');          // Ex.: "checkView", "checkInsert", etc.
    var checked = $(this).is(':checked');     // Estado do checkbox do grupo

    // Itera sobre todas as linhas da tabela (pode ser todos ou somente a página atual)
    table.rows().every(function () {
        var rowData = this.data();
        // Verifica se a linha pertence ao mesmo grupo
        if (rowData.groupDescription === group) {
            var $row = $(this.node());
            // Busca pelo input correspondente usando o atributo name e atualiza seu estado
            $row.find('input[name="' + perm + '"]').prop('checked', checked);

            // Se necessário, atualize também o objeto de dados (opcional)
            var roleProp = perm.replace('check', 'role');
            if (rowData.hasOwnProperty(roleProp)) {
                rowData[roleProp] = checked;
                //this.data(rowData);
            }
        }
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

        var data = [];

        $('#tableMain tbody tr').each(function () {
            var row = $(this);
            var roleView = row.find('td:nth-child(3) input[type="checkbox"]').is(':checked');
            var roleInsert = row.find('td:nth-child(4) input[type="checkbox"]').is(':checked');
            var roleUpdate = row.find('td:nth-child(5) input[type="checkbox"]').is(':checked');
            var roleDelete = row.find('td:nth-child(6) input[type="checkbox"]').is(':checked');
            var rolePrint = row.find('td:nth-child(7) input[type="checkbox"]').is(':checked');
            var roleAdmin = row.find('td:nth-child(8) input[type="checkbox"]').is(':checked');

            var newRecord = {
                "id": row.attr('id'),
                "roleView": roleView,
                "roleInsert": roleInsert,
                "roleUpdate": roleUpdate,
                "roleDelete": roleDelete,
                "rolePrint": rolePrint,
                "roleAdmin": roleAdmin
            };

            data.push(newRecord);
        });

        $("#formRole").val(JSON.stringify(data));

        return true;

    },
    invalidHandler: function (e, validation) {
        $("#btnSave").prop('disabled', false);
    }
})