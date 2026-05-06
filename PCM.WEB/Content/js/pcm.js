'use strict';

$.ajaxPrefilter(function (options, originalOptions, xhr) {

    const isExternal = options.url.startsWith("http");

    if (!isExternal) {

        const token = $('input[name="__RequestVerificationToken"]').val();

        if (token) {
            xhr.setRequestHeader("RequestVerificationToken", token);
        }
    }
});

function loadGridMain({
    tableId = "#tableDynamic",
    table = null,
    data = {},
    endpoint,
    editAction = false,
    deleteAction = false,
    warningAction = false,
    customAction = false,
    grupo = "",
    onEdit = null,
    onDelete = null,
    onWarning = null,

    enablePaging = true,
    pageLength = 10,
    enableSearch = true,
    enableExport = true,
    textSearch = "Pesquisar:",
    textNothingRegister = "Nenhum registro encontrado...",

    customButtons = [],

    enableChild = false,
    childRender = null,

    onLoaded = null
}) {

    // =========================
    // RESET TABLE
    // =========================
    if ($.fn.DataTable.isDataTable(tableId)) {
        $(tableId).DataTable().destroy();
        $(tableId).empty();
    }

    const token = $('input[name="__RequestVerificationToken"]').val();

    // =========================
    // AJAX LOAD
    // =========================
    $.ajax({
        url: endpoint,
        type: "POST",
        data: {
            ...data,
            __RequestVerificationToken: token
        },
        success: function (response) {

            const rows = response.data || [];
            const columnsResponse = response.columns || [];
            const groupDefs = response.groupBy || [];

            if (onLoaded) {
                onLoaded(response, rows);
            }

            // =========================
            // COLUMNS
            // =========================
            let dynamicColumns = columnsResponse.map(col => ({
                data: col.data,
                title: col.title,
                visible: col.visible ?? true,
                orderable: col.orderable ?? true,
                width: col.width ?? "",
                className: col.align ? "text-" + col.align : "",
                defaultContent: ""
            }));

            // =========================
            // CHILD CONTROL (+ / -)
            // =========================
            if (enableChild) {
                dynamicColumns.unshift({
                    className: "details-control text-center",
                    orderable: false,
                    data: null,
                    width: "30px",
                    defaultContent: "<i class='fa fa-plus'></i>"
                });
            }

            // =========================
            // ACTIONS
            // =========================
            if (editAction || deleteAction || warningAction || customAction) {

                let buttons = [];

                if (editAction) {
                    buttons.push({
                        class: "btn-edit btn btn-sm btn-primary",
                        icon: "fa fa-pencil",
                        action: "edit"
                    });
                }

                if (deleteAction) {
                    buttons.push({
                        class: "btn-delete btn btn-sm btn-danger",
                        icon: "fa fa-times",
                        action: "delete"
                    });
                }

                if (warningAction) {
                    buttons.push({
                        class: "btn-warning-view btn btn-sm btn-outline-secondary",
                        icon: "fa fa-warning text-warning",
                        action: "warning"
                    });
                }

                if (customAction) {
                    customButtons.forEach(btn => {
                        buttons.push({
                            class: btn.class || "btn btn-sm btn-secondary",
                            icon: btn.icon || "",
                            action: btn.action,
                            title: btn.title || "",
                            visible: btn.visible,
                            onClick: btn.onClick
                        });
                    });
                }

                dynamicColumns.push({
                    data: null,
                    orderable: false,
                    width: "40px",
                    className: "text-center",
                    render: function (data, type, row) {
                        const html = buttons
                            .filter(b => {
                                if (!b.visible) return true;
                                return b.visible(row);
                            }).map(b => `<button type="button" class="${b.class}" data-action="${b.action}" title="${b.title}"><i class="${b.icon}"></i></button>`).join("");
                        return `<div class='btn-group'>${html}</div>`;
                    }
                });
            }

            // =========================
            // EXPORT BUTTONS (ORIGINAL)
            // =========================
            let buttonsConfig = [];

            if (enableExport) {
                buttonsConfig = [
                    {
                        extend: 'copy',
                        text: "<i class='fa fa-copy text-primary'></i>",
                        titleAttr: messages.clickToCopy,
                        className: 'btn btn-sm btn-outline-light mb-2'
                    },
                    {
                        extend: 'excel',
                        text: "<i class='fa fa-file-excel-o text-success'></i>",
                        titleAttr: messages.clickToExcel,
                        className: 'btn btn-sm btn-outline-light mb-2'
                    },
                    {
                        extend: 'pdf',
                        text: "<i class='fa fa-file-pdf-o text-danger'></i>",
                        titleAttr: messages.clickToPdf,
                        className: 'btn btn-sm btn-outline-light mb-2'
                    },
                    {
                        extend: 'colvis',
                        text: "<i class='fa fa-columns text-dark'></i>",
                        titleAttr: messages.clickToConfig,
                        className: 'btn btn-sm btn-outline-light mb-2'
                    }
                ];
            }

            // =========================
            // DATATABLE
            // =========================
            const dt = $(tableId).DataTable({
                data: rows,
                columns: dynamicColumns,

                paging: enablePaging,
                pageLength: pageLength,
                searching: enableSearch,
                ordering: false,
                autoWidth: false,
                deferRender: true,

                // LAYOUT ORIGINAL
                dom: enableExport
                    ? "<'row'<'col-sm-12 col-md-6'B><'col-sm-12 col-md-6'f>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>"
                    : "<'row'<'col-sm-12'tr>>",

                buttons: buttonsConfig,

                // LANGUAGE ORIGINAL
                language: {
                    info: "",
                    infoEmpty: "",
                    lengthMenu: "",
                    zeroRecords: textNothingRegister,
                    search: textSearch
                },

                drawCallback: function () {
                    const api = this.api();
                    renderGroups(api, groupDefs, dynamicColumns.length, tableId);
                    applyCollapse(api, groupDefs[0]?.column, tableId);
                }
            });

            // =========================
            // EVENTS (EDIT / DELETE / WARNING)
            // =========================
            bindGridEvents(dt, { onEdit, onDelete, onWarning, customButtons }, tableId);

            // =========================
            // CHILD EVENT (+ / -)
            // =========================
            if (enableChild) {

                $(`${tableId} tbody`).off('click', 'td.details-control');

                $(`${tableId} tbody`).on('click', 'td.details-control', function () {

                    const tr = $(this).closest('tr');
                    const row = dt.row(tr);
                    const icon = $(this).find("i");

                    if (row.child.isShown()) {

                        row.child.hide();
                        tr.removeClass('shown');
                        icon.removeClass('fa-minus').addClass('fa-plus');

                    } else {

                        let content = "";

                        if (childRender) {
                            content = childRender(row.data());
                        } else {
                            content = "<div class='p-2'>No child content</div>";
                        }

                        row.child(content).show();
                        tr.addClass('shown');
                        icon.removeClass('fa-plus').addClass('fa-minus');
                    }
                });
            }
        }
    });
}

function renderGroups(api, groupDefs, colspan, tableId) {

    $(`${tableId} tbody tr.dt-group-row`).remove();

    if (!groupDefs.length) return;

    let lastValue = null;

    api.rows({ page: 'current' }).every(function () {

        const row = this.data();
        const groupValue = row[groupDefs[0].column];

        if (groupValue !== lastValue) {

            const $tr = $("<tr/>")
                .addClass("dt-group-row")
                .attr("data-group", groupValue);

            const $td = $("<td/>")
                .attr("colspan", colspan)
                .html(`<strong>${groupValue}</strong>`);

            $tr.append($td);
            $(this.node()).before($tr);

            lastValue = groupValue;
        }
    });

    $(`${tableId} tbody tr.dt-group-row`)
        .off('click')
        .on('click', function () {

            const stateKey = "dt-group-collapse:" + window.location.pathname;

            let state = JSON.parse(localStorage.getItem(stateKey)) || {};

            const group = $(this).data("group");
            const collapsed = $(this).hasClass("collapsed");

            state[group] = !collapsed;

            localStorage.setItem(stateKey, JSON.stringify(state));

            $(this).toggleClass("collapsed");

            const api = $(tableId).DataTable();

            api.rows().every(function () {

                const row = this.data();

                if (row[groupDefs[0].column] === group) {
                    $(this.node()).toggle(!collapsed);
                }
            });
        });
}

function applyCollapse(api, groupColumn, tableId) {

    if (!groupColumn) return;

    const stateKey = "dt-group-collapse:" + window.location.pathname;

    let state = JSON.parse(localStorage.getItem(stateKey)) || {};

    api.rows({ page: 'current' }).every(function () {

        const row = this.data();
        const node = this.node();

        if (state[row[groupColumn]]) {
            node.style.display = "none";
        } else {
            node.style.display = "";
        }
    });

    $(`${tableId} tbody tr.dt-group-row`).each(function () {

        const group = $(this).data("group");
        $(this).toggleClass("collapsed", !!state[group]);
    });
}

function bindGridEvents(table, callbacks, tableId) {

    const { onEdit, onDelete, onWarning, customButtons } = callbacks;

    const $tbody = $(`${tableId} tbody`);

    $tbody.off('click');

    $tbody.on('click', 'button', function (e) {

        e.preventDefault();
        e.stopPropagation();

        const $tr = $(this).closest('tr');

        if ($tr.hasClass('dt-group-row')) return;

        const row = table.row($tr);

        if (!row.node()) return;

        const rowData = row.data();

        const action = $(this).data("action");

        if (action === "edit" && onEdit) {
            onEdit(rowData);
        }

        if (action === "delete" && onDelete) {
            onDelete(rowData);
        }

        if (action === "warning" && onWarning) {
            onWarning(rowData);
        }

        const custom = customButtons.find(x => x.action === action);

        if (custom && custom.onClick) {
            custom.onClick(rowData);
        }

    });
}

function showWarning(data) {

    if ($.fn.DataTable.isDataTable('#tableWarning')) {
        $('#tableWarning').DataTable().clear().destroy();
    }

    $('#tableWarning tbody').empty();

    $('#tableWarning').DataTable({
        lengthChange: false,
        paging: false,
        searching: false,
        scrollX: false,
        autoWidth: false,
        data: data.errorData || [],
        columns: [
            { data: "message", title: "Mensagem" }
        ],
        language: {
            emptyTable: messages.emptyTable,
            info: "",
            infoEmpty: "",
            infoFiltered: ""
        }
    });

    $("#modWarningInfo").modal("show");
}

function gridHasErrors() {

    if (!$.fn.DataTable.isDataTable('#tableDynamic'))
        return false;

    var table = $('#tableDynamic').DataTable();
    var data = table.rows().data().toArray();

    for (var i = 0; i < data.length; i++) {

        if (data[i].errorData &&
            Array.isArray(data[i].errorData) &&
            data[i].errorData.length > 0) {

            return true;
        }
    }

    return false;
}

function validateDB(url, dataFn) {

    var token = $('form input[name="__RequestVerificationToken"]').first().val();

    return {
        url: url,
        type: "POST",
        data: Object.assign(dataFn(), {
            __RequestVerificationToken: token
        }),
        success: function (response) {
            return response;
        }
    };

}

function loadCombo(url, dataFn, targetSelector, valueField = "codigo", textField = "descricao") {

    var token = $('form input[name="__RequestVerificationToken"]').first().val();

    $.ajax({
        type: "POST",
        url: url,
        data: Object.assign(dataFn(), {
            __RequestVerificationToken: token
        }),
        success: function (response) {

            var $target = $(targetSelector);

            $target.empty();
            $target.append('<option value=""></option>');

            if (!response) return;

            $.each(response, function (i, item) {
                $target.append(`<option value="${item[valueField]}">${item[textField]}</option>`);
            });

            $target.trigger('change'); // importante para select2

        }
    });
}

async function rfConfirm({
    title = "Confirm",
    message = "",
    confirmButtonText = "Yes",
    cancelButtonText = "No",
    icon = "question"
}) {

    const result = await Swal.fire({
        title: title,
        text: message,
        icon: icon,
        showCancelButton: true,
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText
    });

    return result.value;
}

async function rfAlert({
    title = "",
    message = "",
    icon = "info",
    confirmButtonText = "Ok"
}) {

    const result = await Swal.fire({
        title: title,
        text: message,
        icon: icon,
        confirmButtonText: confirmButtonText
    });

    return result;

}

async function rfAskNumber(title, maxNumber, placeholder, msgRequiredValue, msgInvalidValue, msgMaxAllowed, confirmButtonText = "OK", cancelButtonText = "Cancel", minNumber = 1, equalNumber = 0) {

    const result = await Swal.fire({
        title: title,
        html: `<input id="swal-value" type="number" class="rf-input-barcode" placeholder="${placeholder}" min="${minNumber}" max="${maxNumber}">`,
        focusConfirm: false,
        showCancelButton: true,
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText,
        allowEnterKey: true,

        onOpen: () => {
            const input = document.getElementById('swal-value');

            input.focus();

            input.addEventListener("keydown", function (e) {
                if (e.key === "Enter") {
                    e.preventDefault();
                    e.stopPropagation(); // importante no SweetAlert
                    Swal.clickConfirm();
                }
            });
        },

        preConfirm: () => {

            const input = document.getElementById('swal-value');
            const value = input.value;

            // remove erro anterior
            input.classList.remove("rf-input-error");

            const oldError = document.getElementById("rf-error");
            if (oldError) oldError.remove();

            if (!value) {
                showError(input, msgRequiredValue);
                return false;
            }

            const qty = parseFloat(value);

            if (isNaN(qty) || qty <= 0) {
                showError(input, msgInvalidValue);
                return false;
            }

            if (qty > maxNumber) {
                showError(input, msgInvalidValue);
                return false;
            }

            if (qty < minNumber) {
                showError(input, msgInvalidValue);
                return false;
            }

            return qty;
        }
    });

    if (result.qty !== "") {
        return result.value;
    }

    return null;
}

async function rfAskInfo(title, placeholder, msgRequiredValue, msgInvalidValue, confirmButtonText = "OK", cancelButtonText = "Cancel", valueConfirmed = "", validateAsync = null) {

    const result = await Swal.fire({
        title: title,
        html: `<input id="swal-info" type="text" class="rf-input-barcode" placeholder="${placeholder}" autocomplete="off" >`,
        focusConfirm: false,
        showCancelButton: true,
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText,
        allowEnterKey: true,

        onOpen: () => {
            const input = document.getElementById('swal-info');

            input.focus();

            input.addEventListener("keydown", function (e) {
                if (e.key === "Enter") {
                    e.preventDefault();
                    e.stopPropagation(); // importante no SweetAlert
                    Swal.clickConfirm();
                }
            });
        },

        preConfirm: async () => {

            const input = document.getElementById('swal-info');
            const value = input.value;

            // remove erro anterior
            input.classList.remove("rf-input-error");

            const oldError = document.getElementById("rf-error");
            if (oldError) oldError.remove();

            if (!value) {
                showError(input, msgRequiredValue);
                return false;
            }

            if (valueConfirmed !== "" && (valueConfirmed !== value)) {
                showError(input, msgInvalidValue);
                return false;
            }

            if (validateAsync) {
                try {
                    const response = await validateAsync(value);

                    if (!response.success) {
                        showError(input, response.message);
                        return false;
                    }

                } catch (err) {
                    Swal.showValidationMessage("Error validating data");
                    return false;
                }
            }


            return value;
        }
    });

    if (result.value !== "") {
        return result.value;
    }

    return null;
}

function showError(input, message) {

    input.classList.add("rf-input-error");

    const error = document.createElement("div");
    error.id = "rf-error";
    error.className = "rf-error-message";
    error.innerHTML = `
        <i class="fas fa-exclamation-circle"></i>
        <span>${message}</span>
    `;

    input.parentNode.appendChild(error);
    input.focus();

    // REMOVE ERRO AO DIGITAR
    input.addEventListener("input", function handler() {
        input.classList.remove("rf-input-error");

        const oldError = document.getElementById("rf-error");
        if (oldError) oldError.remove();

        // remove o listener depois de executar uma vez
        input.removeEventListener("input", handler);
    });
}

function consultCEP({
    cep,
    street,
    district,
    city,
    province
}) {

    const cleanCep = (cep || "").replace(/\D/g, '');

    // limpa campos
    $(street).val("");
    $(district).val("");
    $(city).val("");
    $(province).val("");

    if (cleanCep.length === 0) return;

    if (cleanCep.length !== 8) {
        rfAlert(messages.warning, messages.validCep, "warning", "OK");
        return;
    }

    $.ajax({
        url: `https://viacep.com.br/ws/${cleanCep}/json/`,
        type: 'GET',
        dataType: 'json',
        headers: {},
        success: function (data) {

            if (!data.erro) {

                $(street).val(data.logradouro);
                $(district).val(data.bairro);
                $(city).val(data.localidade);
                $(province).val(data.uf);

            } else {
                rfAlert("ERROR", messages.validCep, "error", "OK");
            }
        },

        error: function () {
            rfAlert("ERROR", messages.validCep, "error", "OK");
        }
    });
}
