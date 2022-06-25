const { src, dest, watch, lastRun, series } = require("gulp");

var sass = require('gulp-sass')(require('sass')),
    group_media = require("gulp-group-css-media-queries"),
    clean_css = require("gulp-clean-css"),
    rename = require("gulp-rename"),
    browserSync = require("browser-sync").create();

var paths = {
    sass: "./wwwroot/sass/*.scss",
    css: "./wwwroot/css",
    cshtml: "./Views/**/*cshtml"
}


function CsHtml() {
    return src(paths.cshtml, {since: lastRun(CsHtml)})
        .pipe(browserSync.stream())
}
exports.CsHtml = CsHtml;

function Sass() {
    return src(paths.sass)
        .pipe(sass())
        .pipe(group_media())
        .pipe(dest(paths.css))
        .pipe(clean_css())
        .pipe(
            rename({
                extname: ".min.css"
            })
        )
        .pipe(dest(paths.css))
        .pipe(browserSync.stream())
}
exports.Sass = Sass;

function myServer() {
    var files = [
        paths.cshtml,
        paths.sass,
        paths.css
    ];

    browserSync.init(files, {
        proxy: 'http://localhost:5104',
        notify: false
    });

    watch(paths.sass, {usePolling: true}, Sass).on('change', browserSync.reload);
    watch(paths.cshtml, {usePolling: true}, CsHtml).on('change', browserSync.reload);
}
exports.myServer = myServer;

exports.default = series(Sass, CsHtml, myServer);