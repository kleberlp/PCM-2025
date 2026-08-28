// ============================================================
//  governanca-ranking-camareira.js — Ranking Geral de Camareiras
//
//  Fonte única do ranking, usada pelo Desempenho da Governança (D03) e pelo
//  dashboard IndexGovernanca: as duas telas mostram o mesmo cálculo, e uma
//  mudança de regra vale para ambas em vez de divergirem com o tempo.
//
//  Nota (1–10) = Produtividade×peso + NC×peso + Retrabalho×peso, com os
//  parâmetros cadastrados em AD07. Elegível a partir de um mínimo de UHs;
//  abaixo disso a camareira aparece como "sem dados", no fim da lista.
//
//  Uso: um container com data-* e um <tbody> alvo —
//    <div id="rk-camareira"
//         data-camareiras='[["Nome",uh,nc,retrab,dias], ...]'
//         data-min-uhs="20" data-vis-rate="0.3"
//         data-peso-prod="0.5" data-peso-nc="0.3" data-peso-ret="0.2"
//         data-alvo="rankingGeralBody"></div>
//
//  Padrão CSP: JS externo, sem handler/estilo inline; células montadas
//  com textContent (nada de HTML concatenado com nome vindo do banco).
// ============================================================

(function () {
    'use strict';

    function fmt1(n) { return n.toFixed(1).replace('.', ','); }

    function celula(texto, classe) {
        var td = document.createElement('td');
        if (classe) { td.className = classe; }
        td.textContent = texto;
        return td;
    }

    function celulaBadge(texto, classeCelula, classeBadge) {
        var td = document.createElement('td');
        if (classeCelula) { td.className = classeCelula; }
        var span = document.createElement('span');
        span.className = classeBadge;
        span.textContent = texto;
        td.appendChild(span);
        return td;
    }

    // Nota, índices e ordenação — a regra que as duas telas compartilham
    function calcular(camareiras, p) {

        var elegiveis = camareiras.filter(function (c) { return c[1] >= p.minUhs; });
        var maxUHs = elegiveis.length
            ? Math.max.apply(null, elegiveis.map(function (c) { return c[1]; }))
            : 1;

        var linhas = camareiras.map(function (c) {
            var uh = c[1], nc = c[2], r = c[3];

            if (uh < p.minUhs) {
                return { nome: c[0], uh: uh, nc: nc, retrab: r, nota: null, iNC: null, iR: null };
            }

            var vis = uh * p.visRate;                                     // quartos vistoriados (estimado)
            var prodComp = (uh / maxUHs) * 100;                           // produtividade relativa ao grupo
            var ncComp = vis > 0 ? Math.max(0, 100 - (nc / vis * 100)) : 0;   // 100 = zero erros
            var retComp = vis > 0 ? Math.max(0, 100 - (r / vis * 100)) : 0;
            var score = prodComp * p.pesoProd + ncComp * p.pesoNC + retComp * p.pesoRet;
            var nota = Math.max(1.0, Math.round(score) / 10);            // escala 1–10, 1 casa

            return {
                nome: c[0], uh: uh, nc: nc, retrab: r, nota: nota,
                iNC: vis > 0 ? nc / vis * 100 : 0,
                iR: vis > 0 ? r / vis * 100 : 0
            };
        });

        // elegíveis por nota desc; inelegíveis (sem dados) por último
        linhas.sort(function (a, b) {
            if (a.nota === null && b.nota === null) { return b.uh - a.uh; }
            if (a.nota === null) { return 1; }
            if (b.nota === null) { return -1; }
            return b.nota - a.nota;
        });

        return linhas;
    }

    function pintar(tbody, linhas) {

        tbody.textContent = '';

        linhas.forEach(function (r, i) {

            var pos = i + 1;
            var rk = pos === 1 ? 'dg-rank-1' : pos === 2 ? 'dg-rank-2' : pos === 3 ? 'dg-rank-3' : 'dg-rank-n';

            var semDados = r.nota === null, nk, sk, st;

            if (semDados) { nk = 'dg-nota-yellow'; sk = 'dg-status-yellow'; st = '— Sem dados'; }
            else if (r.nota >= 9.0) { nk = 'dg-nota-green'; sk = 'dg-status-green'; st = '● Excelente'; }
            else if (r.nota >= 7.0) { nk = 'dg-nota-green'; sk = 'dg-status-green'; st = '● Bom'; }
            else if (r.nota >= 5.0) { nk = 'dg-nota-yellow'; sk = 'dg-status-yellow'; st = '● Regular'; }
            else { nk = 'dg-nota-red'; sk = 'dg-status-red'; st = '● Atenção'; }

            var tr = document.createElement('tr');
            if (semDados) { tr.className = 'dg-sem-dados'; }

            tr.appendChild(celulaBadge(String(pos), '', 'dg-rank-badge ' + rk));
            tr.appendChild(celula(r.nome, 'font-w600'));
            tr.appendChild(celula(String(r.uh), 'text-center font-w700'));
            tr.appendChild(celula(String(r.nc), 'text-center font-w700 u-c-red-mat'));
            tr.appendChild(celula(String(r.retrab), 'text-center font-w700 u-c-orange-deep'));
            tr.appendChild(celula(semDados ? '—' : fmt1(r.iNC) + '%', 'text-center'));
            tr.appendChild(celula(semDados ? '—' : fmt1(r.iR) + '%', 'text-center'));
            tr.appendChild(celulaBadge(semDados ? '—' : fmt1(r.nota), 'text-center', 'dg-nota-badge ' + nk));
            tr.appendChild(celulaBadge(st, 'text-center', 'dg-status-pill ' + sk));

            tbody.appendChild(tr);
        });
    }

    function iniciar(container) {

        var tbody = document.getElementById(container.getAttribute('data-alvo'));
        if (!tbody) { return; }

        var camareiras;
        try {
            camareiras = JSON.parse(container.getAttribute('data-camareiras') || '[]');
        } catch (e) {
            camareiras = [];
        }

        pintar(tbody, calcular(camareiras, {
            minUhs: Number(container.getAttribute('data-min-uhs')) || 0,
            visRate: Number(container.getAttribute('data-vis-rate')) || 0,
            pesoProd: Number(container.getAttribute('data-peso-prod')) || 0,
            pesoNC: Number(container.getAttribute('data-peso-nc')) || 0,
            pesoRet: Number(container.getAttribute('data-peso-ret')) || 0
        }));
    }

    document.addEventListener('DOMContentLoaded', function () {
        var lista = document.querySelectorAll('[data-ranking-camareira]');
        for (var i = 0; i < lista.length; i++) { iniciar(lista[i]); }
    });

    // Para a tela que ja tem os dados em memoria (D03) chamar direto,
    // sem repetir o JSON num data-*.
    window.PcmRankingCamareira = {
        render: function (idTbody, camareiras, parametros) {
            var tbody = document.getElementById(idTbody);
            if (tbody) { pintar(tbody, calcular(camareiras || [], parametros || {})); }
        }
    };

})();
