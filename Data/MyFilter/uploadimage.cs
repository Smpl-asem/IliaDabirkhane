public static class Uploadimage
{
    //Add New File
    public static string Upload(string images)
    {
       
        
        if (string.IsNullOrEmpty(images))
        {
            return "/images/default.jpg";
        }
        else
        {
            byte[] imageBytes;
            if (!TryConvertFromBase64String(images, out imageBytes))
            {
                //set default image
                return "/images/default.jpg";
            }

            string imageName = Guid.NewGuid().ToString() + ".jpg";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
            System.IO.File.WriteAllBytes(imagePath, imageBytes);
            return ("/images/"+imageName);
        }

    }


    //Update File
    public static string UpdateImage(string newImage, string preImage)
    {
        if (string.IsNullOrEmpty(newImage))
        {
            //Delete PreImage
            DeleteImageFromServer(preImage);

            //Set Default Image
            return "/images/default.jpg";
        }
        else
        {
            byte[] imageBytes;
            if (!TryConvertFromBase64String(newImage, out imageBytes))
            {
                //set previous image
                return preImage;
            }

            //Delete PreImage
            DeleteImageFromServer(preImage);

            //Add NewImage
            string imageName = Guid.NewGuid().ToString() + ".jpg";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
            System.IO.File.WriteAllBytes(imagePath, imageBytes);
            return ("/images/" + imageName);
        }
    }


    //Check String is Base64
    private static bool TryConvertFromBase64String(string base64String, out byte[] result )
        {
            try
            {
                // Remove the prefix before decoding if one split 
                // the base64 string into two parts
                var base64Part = base64String.Split(',').Length > 1
                    ? base64String.Split(',')[1]
                    : base64String;
                result = Convert.FromBase64String(base64Part);
                return true;
            }
            catch (Exception ex)
            {
                result = null;
                return false;
            }
        }


    //Delete file from Server
    public static void DeleteImageFromServer(string filePath)
    {
        if(filePath != "/images/default.jpg" && filePath != null )
        {
            //Find Last Index of / Character in filePath
            int lastIndex = filePath.LastIndexOf('/');
            if (lastIndex != -1)
            {
                //Extract FileName With Extension
                var fileName = filePath.Substring(lastIndex + 1);

                //filePath in Server
                var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                //if file exist
                if (System.IO.File.Exists(file))
                {
                    //Delete File
                    System.IO.File.Delete(file);
                }
            }
        }
    }

}