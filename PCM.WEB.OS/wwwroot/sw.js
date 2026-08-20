// ============================================================
//  PCM.WEB.OS - Service Worker (PWA)
//  - Precache do app shell + fallback offline
//  - Navegação: network-first (dados sempre frescos)
//  - Assets estáticos: cache-first com revalidação em segundo plano
//  - Mantém as notificações push já existentes
//  IMPORTANTE: ao alterar este arquivo, suba a versão do CACHE_VERSION.
// ============================================================

const CACHE_VERSION = 'pcm-os-v2';

// Tudo é resolvido a partir do escopo do próprio service worker, e não de "/",
// para o app funcionar também quando publicado em um diretório virtual
const SCOPE_URL = self.registration.scope;                 // ex.: https://host/ ou https://host/OS/
const SCOPE_PATH = new URL(SCOPE_URL).pathname;            // ex.: /        ou /OS/

const url = caminho => new URL(caminho, SCOPE_URL).href;

const OFFLINE_URL = url('offline.html');

// Somente recursos estáticos: nada de HTML de páginas (dependem de sessão/uniqueId)
const PRECACHE_URLS = [
    OFFLINE_URL,
    url('css/bootstrap.min.css'),
    url('css/icons.min.css'),
    url('css/metisMenu.min.css'),
    url('css/app.css'),
    url('js/jquery.min.js'),
    url('js/bootstrap.bundle.min.js'),
    url('lib/sweet-alert2/sweetalert2.min.css'),
    url('lib/sweet-alert2/sweetalert2.min.js'),
    url('lib/select2/select2.min.css'),
    url('lib/select2/select2.min.js'),
    url('js/pages/ativoFixo/jquery.assetInventory.init.js'),
    url('images/pwa/icon-192.png'),
    url('images/pwa/icon-512.png'),
    url('images/favicon.png')
];

// Pastas tratadas como estáticas (cache-first)
const STATIC_PATHS = ['css/', 'js/', 'lib/', 'images/', 'fonts/', 'scss/'];

function isStaticAsset(u) {
    return STATIC_PATHS.some(p => u.pathname.startsWith(SCOPE_PATH + p));
}

// ---------------- Install: precache tolerante a falhas ----------------
self.addEventListener('install', event => {
    event.waitUntil((async () => {
        const cache = await caches.open(CACHE_VERSION);

        // allSettled: um arquivo ausente não invalida a instalação inteira
        await Promise.allSettled(
            PRECACHE_URLS.map(item => cache.add(new Request(item, { cache: 'reload' })))
        );
    })());
});

// ---------------- Activate: limpa versões antigas ----------------
self.addEventListener('activate', event => {
    event.waitUntil((async () => {
        const keys = await caches.keys();
        await Promise.all(keys.filter(k => k !== CACHE_VERSION).map(k => caches.delete(k)));
        await self.clients.claim();
    })());
});

// ---------------- Atualização controlada pela página ----------------
self.addEventListener('message', event => {
    if (event.data === 'SKIP_WAITING' || (event.data && event.data.type === 'SKIP_WAITING')) {
        self.skipWaiting();
    }
});

// ---------------- Fetch ----------------
self.addEventListener('fetch', event => {
    const request = event.request;

    // Só GET é cacheável; POST (bipagem, uploads) sempre vai à rede
    if (request.method !== 'GET') return;

    const reqUrl = new URL(request.url);

    // Não intercepta outras origens nem o próprio manifesto (start_url por uniqueId)
    if (reqUrl.origin !== self.location.origin) return;
    if (reqUrl.pathname === SCOPE_PATH + 'manifest.webmanifest') return;

    // Navegação: network-first com fallback offline
    if (request.mode === 'navigate') {
        event.respondWith((async () => {
            try {
                return await fetch(request);
            } catch (e) {
                const cache = await caches.open(CACHE_VERSION);
                return (await cache.match(OFFLINE_URL)) || Response.error();
            }
        })());
        return;
    }

    // Estáticos: cache-first + revalidação em segundo plano
    if (isStaticAsset(reqUrl)) {
        event.respondWith((async () => {
            const cache = await caches.open(CACHE_VERSION);
            const cached = await cache.match(request);

            const network = fetch(request).then(response => {
                if (response && response.ok) cache.put(request, response.clone());
                return response;
            }).catch(() => null);

            return cached || (await network) || Response.error();
        })());
        return;
    }

    // Demais requisições (endpoints de dados): rede, sem cache
});

// ============================================================
//  Notificações push (comportamento já existente)
// ============================================================
self.addEventListener('push', function (event) {
    const data = event.data.json();
    const options = {
        body: data.message,
        icon: url('images/notification_icon.png'),
        badge: url('images/notification_badge.png')
    };

    event.waitUntil(
        self.registration.showNotification(data.title, options)
    );
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();
    event.waitUntil(
        clients.openWindow(SCOPE_URL)
    );
});
