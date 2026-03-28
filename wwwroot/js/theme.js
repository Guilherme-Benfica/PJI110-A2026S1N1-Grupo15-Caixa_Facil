/* ── CaixaFácil — theme.js ─────────────────────────────────────────── */

/**
 * Aplica o tema ao elemento <html>.
 * @param {string} tema  "light" | "dark" | "system"
 */
function applyTheme(tema) {
    const root = document.getElementById('htmlRoot');
    if (!root) return;

    let resolved = tema;
    if (tema === 'system') {
        resolved = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }
    root.setAttribute('data-bs-theme', resolved);

    // Atualiza ícone do botão de tema
    const ic = document.getElementById('icTema');
    if (ic) {
        ic.className = resolved === 'dark'
            ? 'bi bi-sun-fill'
            : 'bi bi-moon-stars-fill';
    }
}

// Lê preferência salva e aplica imediatamente
(function () {
    const saved = localStorage.getItem('caixafacil_tema') || 'system';
    applyTheme(saved);
})();

// Botão de alternância no layout
document.addEventListener('DOMContentLoaded', function () {
    const btn = document.getElementById('btnTema');
    if (btn) {
        btn.addEventListener('click', function () {
            const current = document.getElementById('htmlRoot').getAttribute('data-bs-theme');
            const next    = current === 'dark' ? 'light' : 'dark';
            localStorage.setItem('caixafacil_tema', next);
            applyTheme(next);
        });
    }

    // Ouve mudança de preferência do SO
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function () {
        const saved = localStorage.getItem('caixafacil_tema') || 'system';
        if (saved === 'system') applyTheme('system');
    });
});
