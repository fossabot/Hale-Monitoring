var gulp = require('gulp');
var concat = require('gulp-concat');
var sass = require('gulp-sass');
var sass_bulk = require('gulp-sass-bulk-import');

gulp.task('default', [
    'app', 'vendor'
]);



gulp.task('app', [
  'app-js',
  'app-sass',
  'app-views',
  'app-index'
]);

gulp.task('vendor', [
  'vendor-js',
  'vendor-css',
  'vendor-fonts'
]);


gulp.task('app-js', function() {
  return gulp.src([
    './src/app-module.js',
    './src/app-route.js',
    './src/*.js',
    './src/**/*.js'
  ])
  .pipe(concat('app.js'))
  .pipe(gulp.dest('./dist/js'));
});

gulp.task('app-sass', function() {
  return gulp.src([
    './src/sass/app.scss'
  ])
  .pipe(sass_bulk())
  .pipe(sass())
  .pipe(gulp.dest('./dist/css'));
});

gulp.task('app-index', function() {
  return gulp.src([
    './src/index.html'
  ])
  .pipe(gulp.dest('./dist'));
});

gulp.task('app-views', function() {
  return gulp.src([
    './src/views/*.html',
    './src/views/**/*.html'
  ])
  .pipe(gulp.dest('./dist/views'));
});

gulp.task('vendor-js', function() {
  return gulp.src([
    './node_modules/chartjs/chart.js',
    './node_modules/angular/angular.js',
    './node_modules/angular-ui-router/release/angular-ui-router.js',
    './node_modules/angular-storage/dist/angular-storage.js',
    './node_modules/angular-gravatar/build/angular-gravatar.js',
    './node_modules/angular-drag-and-drop-lists/angular-drag-and-drop-lists.js',
    './node_modules/chart.js/dist/Chart.min.js',
    './node_modules/angular-chart.js/dist/angular-chart.js',
    './bower_components/jquery/dist/jquery.js',
    './node_modules/bootstrap/dist/js/bootstrap.js'
  ])
  .pipe(concat('vendor.js'))
  .pipe(gulp.dest('./dist/js'))
});

gulp.task('vendor-css', function() {
  return gulp.src([
    './node_modules/bootstrap/dist/css/bootstrap.css',
    './node_modules/font-awesome/css/font-awesome.css',
  ])
  .pipe(concat('vendor.css'))
  .pipe(gulp.dest('./dist/css'));
});


gulp.task('vendor-fonts', function() {
  return gulp.src([
    './node_modules/bootstrap/dist/fonts/*',
    './node_modules/font-awesome/fonts/*'
  ])
  .pipe(gulp.dest('./dist/fonts'));
});
