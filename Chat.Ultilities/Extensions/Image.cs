
using Chat.Ultilities.Loger;
using System;
using System.Drawing;
namespace Chat.Ultilities.Extensions
{
    public static class Image
    {
        #region 裁剪图片
        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="posX">裁剪位置横坐标</param>
        /// <param name="posY">裁剪位置纵坐标</param>
        public static bool ImageRepair(string originalImagePath, string thumbnailPath, int width, int height, int posX, int posY)
        {

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            bool success = false;
            //新建一个bmp图片
            System.Drawing.Image bitmap = new Bitmap(width, height);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, width, height), posX, posY, width, height, GraphicsUnit.Pixel);

            try
            {
                //以Png格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
                success = true;
            }
            catch (Exception ex)
            {
                Log.Exception("Image", "ImageRepair", ex);
                success = false;
                return success;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            return success;
        }
        #endregion

        #region 缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        public static bool Thumbnail(string originalImagePath, string thumbnailPath, int width, int height)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            bool success = false;
            //新建一个bmp图片
            System.Drawing.Image bitmap = new Bitmap(width, height);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, width, height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                success = true;
            }
            catch (Exception ex)
            {
                Log.Exception("Image", "Thumbnail", ex);
                return success;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            return success;
        }
        #endregion
    }
}
