
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);
const basePath = window.location.pathname.split("/").slice(0, 2).join("/");

$(document).ready(function () {

    $("#unidade").change(function () {
        table.ajax.reload(null, false);
    });

    var table = $('#tableMain').DataTable({
        select: {
            selector: 'td:not(:first-child)',
            style: 'os'
        },
        searching: false,
        fixedColumns: {
            start: 0,
            end: 1
        },
        lengthChange: false,
        pageLength: 15,
        paging: false,
        processing: true,
        scrollCollapse: false,
        serverSide: false,
        ajax: {
            url: `${basePath}/LoadEnxoval`,
            type: "POST",
            datatype: "json",
            data: function (d) {
                d.empresa = $('#empresa').val(),
                d.unidade = ($('#unidade').val() == "") ? -1 : $('#unidade').val()
            },
            dataSrc: ""
        },
        dom: "<'row'<'col-sm-12 col-md-6'B><'col-sm-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        buttons: [
            {
                extend: 'copy',
                text: "<i class='fa fa-copy text-primary'></i>",
                titleAttr: messages.clickToCopy,
                className: 'btn btn-sm btn-secondary'
            },
            {
                extend: 'excel',
                text: "<i class='fa fa-file-excel-o text-success'></i>",
                titleAttr: messages.clickToExcel,
                className: 'btn btn-sm btn-secondary'
            },
            {
                extend: 'pdf',
                text: "<i class='fa fa-file-pdf-o text-danger'></i>",
                titleAttr: messages.clickToPdf,
                className: 'btn btn-sm btn-secondary'
            },
            {
                extend: 'colvis',
                text: "<i class='fa fa-columns text-black'></i>",
                titleAttr: messages.clickToConfig,
                className: 'btn btn-sm btn-outline-light waves-light waves-effect dropdown-toggle tippy-btn mb-2',
                columns: [0, 1]
            }
        ],
        columns: [
            { data: "descricao" },
            {
                data: "quantidade",
                render: function (data, type, row, meta) {
                    return `<input type="text" name="quantidade" class="form-control text-center" value="${data}" data-rowindex="${meta.row}" />`;
                }
            },
            {
                data: "quantidadeUso",
                render: function (data, type, row, meta) {
                    return `<input type="text" name="quantidadeUso" class="form-control text-center" value="${data}" data-rowindex="${meta.row}" />`;
                }
            },
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: "",
        },
        order: [[0, 'asc']],
        columnDefs: [
            { className: 'text-center', targets: [1, 2] },
            { width: '150px', targets: [1, 2] }
        ],
    });

    // Reload DataTable
    $('#btnSave').click(function () {
        $("#form").submit();
    });

    $('#form').validate({
        onkeyup: false,
        onclick: false,
        ignore: [],
        errorClass: 'invalid-feedback animated fadeInDown',
        errorElement: 'div',
        errorPlacement: function (error, e) {
            jQuery(e).parents('.form-group > div').append(error);
        },
        highlight: function (e) {
            jQuery(e).closest('.form-group > div').removeClass('is-invalid').addClass('is-invalid');
        },
        success: function (e) {
            jQuery(e).closest('.form-group > div').removeClass('is-invalid');
            jQuery(e).remove();
        },
        rules: {
            'unidade': {
                required: true
            },
            'data': {
                required: true
            }
        },
        messages: {
            'unidade': {
                required: messages.requiredField
            },
            'data': {
                required: messages.requiredField
            }
        },
        submitHandler: function (form) {

            table.rows().every(function () {
                let rowDataEnxoval = this.data();
                let quantidadeEnxoval = $(this.node()).find('input[type="text"][name="quantidade"]').val();
                rowDataEnxoval.quantidade = quantidadeEnxoval;
                this.data(rowDataEnxoval);
            });

            let dataArrayEnxoval = table.rows().data().toArray();
            let jsonStringEnxoval = JSON.stringify(dataArrayEnxoval);
            $('#jsonEnxoval').val(jsonStringEnxoval);

            return true;

        }
    });

});
