var gulp = require('gulp');


// 将bower的库文件对应到指定位置
gulp.task('buildlib',function(){

  gulp.src('./bower_components/art-dialog/dist/dialog-min.js')
      .pipe(gulp.dest('./libs/js/'))

  gulp.src('./bower_components/jquery/dist/jquery.min.js')
      .pipe(gulp.dest('./libs/js/'))
      
  gulp.src('./bower_components/jquery/dist/jquery.min.map')
  .pipe(gulp.dest('./libs/js/'))

  gulp.src('./bower_components/bootstrap/dist/js/bootstrap.min.js')
      .pipe(gulp.dest('./libs/js/'))

  //--------------------------css-------------------------------------
  gulp.src('./bower_components/art-dialog/css/ui-dialog.css')
      .pipe(gulp.dest('./libs/css/'))

  gulp.src('./bower_components/bootstrap/dist/css/bootstrap.min.css')
      .pipe(gulp.dest('./libs/css/'))

  gulp.src('./bower_components/bootstrap/dist/css/bootstrap.css.map')
      .pipe(gulp.dest('./libs/css/'))
});

// 定义develop任务在日常开发中使用
gulp.task('develop',function(){
  gulp.run('buildlib');
});

// 定义一个prod任务作为发布或者运行时使用
gulp.task('prod',function(){
  gulp.run('buildlib','stylesheets','javascripts');
});

// gulp命令默认启动的就是default认为,这里将clean任务作为依赖,也就是先执行一次clean任务,流程再继续.
gulp.task('default', function() {
  gulp.run('develop');
});