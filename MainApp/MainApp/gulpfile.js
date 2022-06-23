"use strict"

var gulp = require("gulp"),
    scss = require('gulp-sass')(require('sass')),
    group_media = require("gulp-group-css-media-queries"),
    clean_css = require("gulp-clean-css"),
    rename = require("gulp-rename");

var paths = {
    webroot: "./wwwroot/"
}

function sass() {
    return gulp.src(paths.webroot + '/sass/*.scss')
        .pipe(scss())
        .pipe(group_media())
        .pipe(gulp.dest(paths.webroot + '/css'))
        .pipe(clean_css())
        .pipe(
            rename({
                extname: ".min.css"
            })
        )
        .pipe(gulp.dest(paths.webroot + '/css'))
}


exports.default = sass