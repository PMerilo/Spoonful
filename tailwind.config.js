/** @type {import('tailwindcss').Config} */
module.exports = {
    prefix: 'tw-',
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml'
    ],
    theme: {
        extend: {
            boxShadow: {
                'dark': '2px 3px rgb(0 0 0 / 1);',
            }
        },
    },
    plugins: [],
    important: true,
    corePlugins: {
        preflight: false
        }

}
