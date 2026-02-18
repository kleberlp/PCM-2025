
const messagesData = document.getElementById('resource-messages').getAttribute('data-messages');
const messages = JSON.parse(messagesData);

const { origin, pathname } = window.location;

const segments = pathname.split('/').filter(Boolean);

const basePath = document.baseURI.replace(/\/$/, '');

var table = null;
var currentConfig = null;
var currentStateKey = null;
var groupDefs = null;

//const basePath = window.location.pathname.split("/").slice(0, 2).join("/");

$(document).ready(function () {

    var inputFile = $("#inputFile"); 

    $(".custom-file-input").on('change', function () {
        var filename = document.getElementById("inputFile").files[0].name;
        $(this).next(".custom-file-label").addClass("selected").html(filename);
    });

    inputFile.change(function () {

        if ( inputFile.val() != "") {
            $("#btnUpload").prop('disabled', false);
        } else {
            $("#btnUpload").prop('disabled', true);
        }

    });

    // Download Excel
    $('#btnDownload').click(function () {
        var form = $('<form>', {
            method: 'POST',
            action: messages.downloadChecklistExcel
        });

        form.append($('<input>', {
            type: 'hidden',
            name: 'tipoChecklist',
            value: $('#tipoChecklist').val()
        }));

        form.append($('<input>', {
            type: 'hidden',
            name: 'codigoUnidade',
            value: $('#unidade').val()
        }));

        form.append($('<input>', {
            type: 'hidden',
            name: 'uniqueId',
            value: $('#uniqueId').val()
        }));

        $('body').append(form);
        form.submit();
        form.remove();
    });

    // Download Excel
    $('#btnUpload').on('click', function (e) {

        e.preventDefault();

        // Pegar o arquivo do input
        var fileInput = $('#inputFile')[0].files[0];

        // Verifica se um arquivo foi selecionado
        if (!fileInput) {
            Swal.fire(messages.uploadError, messages.pleaseSelectFile, 'error');
            return;
        }

        // Criar um objeto FormData
        var formData = new FormData();
        formData.append('file', fileInput);
        formData.append('codigoUnidade', ($('#unidade').val() === "" ? -1 : $('#unidade').val()));
        formData.append('tipoChecklist', $("#tipoChecklist option:selected").text());

        // Exibe o SweetAlert2 de loading
        Swal.fire({
            title: 'Uploading...',
            html: '<i class="la la-refresh text-secondary la-spin progress-icon-spin"></i>',
            allowOutsideClick: false,
            allowEscapeKey: false,
            showConfirmButton: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        // Envio Ajax
        $.ajax({
            url: $('#btnUpload').data('url'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    Swal.close();
                    $("#uniqueId").val(response.uniqueId);
                    loadGrid();
                } else {
                    Swal.close();
                    Swal.fire(messages.uploadError, response.message, 'error');
                }
            },
            error: function (xhr, status, error) {
                Swal.fire(messages.uploadError, messages.errorOnSentFile, 'error');
            }
        });
    });

    function loadGrid() {

        localStorage.clear();

        if (table) {
            table.destroy();
            table = null;
        }

        $.ajax({
            url: messages.loadChecklist,
            type: "POST",
            data: {
                uniqueId: $("#uniqueId").val(),
                tipoChecklist: $("#tipoChecklist").val()
            },
            success: function (response) {

                var data = response.data || [];
                var columnsResponse = response.columns || [];

                var groupDefsAll = (response.groupBy || [])
                    .slice()
                    .sort(function (a, b) { return (a.Level || 0) - (b.Level || 0); });

                // remove datatable antigo + DOM
                if ($.fn.DataTable.isDataTable('#tableDynamic')) {
                    $('#tableDynamic').DataTable().destroy();
                    $('#tableDynamic').empty();
                }

                // --- helpers ---
                function normalize(v) {
                    if (v === null || v === undefined) return "";
                    return v.toString().trim().replace(/\s+/g, " ").toUpperCase();
                }

                // níveis efetivos: só considera nível se existir algum valor preenchido no dataset
                var groupDefs = groupDefsAll.filter(function (g) {
                    return data.some(function (row) {
                        return normalize(row[g.Column]) !== "";
                    });
                });

                var groupColumns = groupDefs.map(function (g) { return g.Column; });

                // frozen
                var frozenCount = 0;

                // columns
                var dynamicColumns = columnsResponse.map(function (col) {
                    if (col.Frozen) frozenCount++;

                    var isGrouped = groupColumns.indexOf(col.Data) >= 0;

                    return {
                        data: col.Data,
                        title: col.Title,
                        visible: isGrouped ? false : !!col.Visible, // grouped sempre invisível
                        width: col.Width || null,
                        orderable: col.Orderable !== false,
                        className: col.Align ? "text-" + col.Align : "",
                        defaultContent: ""
                    };
                });

                // estado por checklist
                currentStateKey = "dt_group_state:" + window.location.pathname + ":tipo=" + ($("#tipoChecklist").val() || "");

                function readState() {
                    try { return JSON.parse(localStorage.getItem(currentStateKey) || "{}"); }
                    catch (e) { return {}; }
                }

                function writeState(obj) {
                    localStorage.setItem(currentStateKey, JSON.stringify(obj));
                }

                function buildKey(level, path) {
                    return "L" + level + "|" + path.slice(0, level + 1).join("||");
                }

                function getPath(row) {
                    // NÃO remove níveis (não usa filter)
                    // Regra: se nível N estiver vazio, a hierarquia para ali.
                    var path = groupColumns.map(function (col) {
                        return normalize(row[col]);
                    });
                    return path;
                }

                function countDirect(rowsData, level, parentPath) {

                    // Último nível de agrupamento -> contar linhas (perguntas) dentro do grupo
                    if (level >= groupColumns.length - 1) {
                        var count = 0;

                        for (var i = 0; i < rowsData.length; i++) {
                            var p = getPath(rowsData[i]);

                            // precisa bater o prefixo do parentPath
                            var ok = true;
                            for (var j = 0; j < parentPath.length; j++) {
                                if (p[j] !== parentPath[j]) { ok = false; break; }
                            }

                            if (ok) count++;
                        }

                        return count;
                    }

                    // Níveis intermediários -> contar filhos diretos distintos (próximo nível)
                    var nextLevel = level + 1;
                    var set = {};

                    for (var i = 0; i < rowsData.length; i++) {
                        var p = getPath(rowsData[i]);

                        // prefixo
                        var ok = true;
                        for (var j = 0; j < parentPath.length; j++) {
                            if (p[j] !== parentPath[j]) { ok = false; break; }
                        }
                        if (!ok) continue;

                        // conta o valor do nível filho (se existir)
                        if (p.length > nextLevel) {
                            set[p[nextLevel]] = true;
                        }
                    }

                    return Object.keys(set).length;
                }


                function applyCollapse(api, animate) {
                    var state = readState();

                    api.rows({ page: 'current' }).every(function () {
                        var row = this.data();
                        var node = this.node();

                        var path = getPath(row);

                        var hidden = false;

                        for (var level = 0; level < path.length; level++) {
                            var key = buildKey(level, path.slice(0, level + 1));
                            if (state[key] === true) { hidden = true; break; }
                        }

                        if (hidden) {
                            if (animate) $(node).stop(true, true).fadeOut(120);
                            else node.style.display = "none";
                        } else {
                            if (animate) $(node).stop(true, true).fadeIn(120);
                            else node.style.display = "";
                        }
                    });
                }

                function renderGroups(api) {

                    $('#tableDynamic tbody tr.dt-group-row').remove();
                    if (!groupColumns.length) return;

                    var state = readState();
                    var rowsData = api.rows({ page: 'current' }).data().toArray();
                    var rowsNodes = api.rows({ page: 'current' }).nodes();

                    var lastPath = [];

                    for (var i = 0; i < rowsData.length; i++) {

                        var row = rowsData[i];
                        var node = rowsNodes[i];

                        var path = getPath(row);

                        for (var level = 0; level < path.length; level++) {

                            var currentValue = path[level];

                            // compara nível por nível
                            if (lastPath[level] !== currentValue) {

                                // reset níveis abaixo (essencial)
                                lastPath = lastPath.slice(0, level);
                                lastPath[level] = currentValue;

                                var def = groupDefs[level] || {};
                                var parentPath = path.slice(0, level + 1);
                                var key = buildKey(level, parentPath);

                                var collapsed = state[key] === true;

                                var icon = def.Collapsible
                                    ? (collapsed ? "<i class='fa fa-chevron-right'></i>" : "<i class='fa fa-chevron-down'></i>")
                                    : "<i class='fa fa-angle-right'></i>";

                                var indent = level * 18;

                                var countText = "";
                                if (def.ShowCount) {
                                    var qtd = countDirect(rowsData, level, parentPath);
                                    countText = " <span class='text-muted'>(" + qtd + ")</span>";
                                }

                                var css = def.CssClass || "";

                                var $tr = $("<tr/>")
                                    .addClass("dt-group-row")
                                    .addClass("level-" + level)
                                    .attr("data-key", key)
                                    .attr("data-level", level)
                                    .attr("data-collapsible", def.Collapsible ? "1" : "0");

                                var $td = $("<td/>")
                                    .attr("colspan", dynamicColumns.length)
                                    .css("padding-left", (12 + indent) + "px")
                                    .html(
                                        "<span class='dt-group-icon' style='display:inline-block;width:18px;'>" + icon + "</span>" +
                                        "<strong>" + currentValue + "</strong>" + countText
                                    );

                                $tr.append($td);
                                $(node).before($tr);
                            }
                        }
                    }

                    // toggle collapse
                    $('#tableDynamic tbody tr.dt-group-row').off('click').on('click', function () {

                        var collapsible = $(this).data("collapsible") == 1;
                        if (!collapsible) return;

                        var key = $(this).data("key");
                        var st = readState();
                        st[key] = !st[key];
                        writeState(st);

                        // atualiza ícone sem redesenhar tudo
                        var isCollapsed = st[key] === true;
                        $(this).find(".dt-group-icon").html(isCollapsed ? "<i class='fa fa-chevron-right'></i>" : "<i class='fa fa-chevron-down'></i>");

                        applyCollapse(api, true);
                    });
                }

                // DataTable config
                currentConfig = {
                    data: data,
                    columns: dynamicColumns,
                    searching: false,
                    paging: false,
                    processing: false,
                    ordering: false,            // importante: DOM previsível
                    autoWidth: false,
                    scrollX: frozenCount > 0,   // se tiver frozen, precisa scrollX
                    deferRender: true,
                    fixedColumns: frozenCount > 0 ? { leftColumns: frozenCount } : false,
                    drawCallback: function () {
                        var api = this.api();
                        renderGroups(api);         // SEM bloqueio _groupRendered
                        applyCollapse(api, false); // idempotente
                    }
                };

                table = $('#tableDynamic').DataTable(currentConfig);

                $("#checklistView").show();

            }
        });
    }

    $.validator.addMethod("validPK", function (value, element) {

        var result = false;

        jQuery.ajax({
            method: "POST",
            url: "ValidaChecklist",
            async: false,
            data: {
                "descricao": value,
                "codigo": $("#codigo").val()
            },
            dataType: "json",
            success: function (response) {
                result = response;
            }
        });

        return result;

    }, messages.validPK);

    $('#form').validate({
        ignore: ".ignore-validation",
        errorClass: 'invalid-feedback animated fadeInDown',
        errorElement: 'div',
        onkeyup: false,
        onclick: false,
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
            'tipoChecklist': {
                required: true
            },
            'descricao': {
                required: true,
                validPK: true
            }
        },
        messages: {
            'tipoChecklist': {
                required: messages.campoObrigatorio
            },
            'descricao': {
                required: messages.campoObrigatorio,
                validPK: messages.validaBancoDados
            }
        },
        submitHandler: function (form) {
            return true;
        },
        invalidHandler: function (e, validation) {
            $("#btnSave").prop('disabled', false);
        }
    });

    loadGrid();

    $("#tipoChecklist").prop("disabled", true)
});
