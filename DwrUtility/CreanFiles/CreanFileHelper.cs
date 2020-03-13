using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DwrUtility.CreanFiles
{
    /// <summary>
    /// 定时删除文件
    /// </summary>
    public class CreanFileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Start(CreanFileParam model)
        {
            #region 检查参数
            if (model == null)
            {
                throw new Exception("没有传参");
            }

            if (model.FileTimes == null || model.FileTimes.Count == 0)
            {
                model.Log?.Invoke(new Exception("没有设置文件夹路径"));
                return;
            }

            var fts = new List<FileTime>();
            foreach (var ft in model.FileTimes)
            {
                var dirs = new List<string>();
                foreach (var dir in ft.Directories)
                {
                    if (Directory.Exists(dir))
                    {
                        dirs.Add(dir);
                    }
                    else
                    {
                        model.Log?.Invoke(new Exception($"文件夹路径设置有误：{dir}"));
                    }
                }
                ft.Directories = dirs;

                if (ft.DeleteTime.Ticks > 0 && ft.Directories.Count > 0)
                {
                    fts.Add(ft);
                }
            }

            if (fts.Count == 0)
            {
                return;
            }

            model.FileTimes = fts;
            #endregion

            Timer timer = null;
            try
            {
                timer = new Timer(state =>
                {
                    var obj = (CreanFileParam)state;
                    Executes(obj);
                }, model, model.DueTime, model.Period);
            }
            catch (Exception ex)
            {
                model.Log?.Invoke(ex);
                timer?.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private static void Executes(CreanFileParam model)
        {
            try
            {
                model.FileTimes.ForEach(ft =>
                {
                    Execute(model, ft);
                });
            }
            catch (Exception ex)
            {
                model.Log?.Invoke(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ft"></param>
        private static void Execute(CreanFileParam model, FileTime ft)
        {
            try
            {
                foreach (var dir in ft.Directories)
                {
                    DirectoryHelper.GetDirectoryFiles(dir, out var dirs, out var files);
                    foreach (var file in files)
                    {
                        var fi = new FileInfo(file);
                        var dt = fi.CreationTime;

                        var tp = (DateTime.Now - dt);
                        if (tp <= ft.DeleteTime)
                        {
                            continue;
                        }

                        if (!DirectoryHelper.DeleteFile(file, false))
                        {
                            model.Log?.Invoke(new Exception($"文件删除失败：{file}"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.Log?.Invoke(ex);
            }
        }
    }
}
