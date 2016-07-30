var gulp = require('gulp');
var concat = require('gulp-concat');

gulp.task('default', [
    'app', 'vendor'
]);



gulp.task('app', [
  'app-js',
  'app-css',
  'app-views',
  'app-views-partials',
  'app-index',
  'app-mocks'
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

gulp.task('app-css', function() {
  return gulp.src([
    './src/app.css'
  ])
  .pipe(gulp.dest('./dist/css'))
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


gulp.task('app-views-partials', function() {
  return gulp.src([
    './src/views/partials/*.html'
  ])
  .pipe(gulp.dest('./dist/views/partials/'));
})

gulp.task('app-mocks', function() {
  return gulp.src('./src/mocks/**')
  .pipe(gulp.dest('./dist/mocks'))
});

gulp.task('vendor-js', function() {
  return gulp.src([
    './node_modules/chartjs/chart.js',
    './node_modules/angular/angular.js',
    './node_modules/angular-route/angular-route.js',
    './node_modules/angular-resource/angular-resource.js',
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
