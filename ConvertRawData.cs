// Convert bitmap to 16 bit GreyScale
private unsafe void ConvertRawData(int width, int height ,int im)
{
    Bitmap bitmap = new Bitmap(bitmap_size, bitmap_size);

    bitmap = (Bitmap)imgsBmp[im];
    
    BitmapData dataRgb = bitmap.LockBits(
        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                     PixelFormat.Format24bppRgb);

    byte* datas24bppRgb = (byte*) dataRgb.Scan0;

    byte[] rawData = new byte[bitmap_size * bitmap_size * 2];

    for (int y = 0; y < bitmap.Height; y++)
    {
        for (int x = 0; x < bitmap.Width; x++)
        {
            //Get bitmap RGB
            int b = (int)(datas24bppRgb[y * bitmap.Width * 3 + x * 3]);
            int g = (int)(datas24bppRgb[y * bitmap.Width * 3 + x * 3 + 1]);
            int r = (int)(datas24bppRgb[y * bitmap.Width * 3 + x * 3 + 2]);

            // Get hu value from setting
            int HU = ConvertHUValue(r, g, b, true);

            int byte1 = HU / 256;
            int byte2 = HU - (byte1 * 256);

            //Byte Swap (Little Endian Format)
            rawData[(y * bitmap.Width + x) * 2] = (byte)byte2;
            rawData[(y * bitmap.Width + x) * 2 + 1] = (byte)byte1;           
        }
    }

    bitmap.UnlockBits(dataRgb);

    imgsRaw[im] = rawData;

    return;
}