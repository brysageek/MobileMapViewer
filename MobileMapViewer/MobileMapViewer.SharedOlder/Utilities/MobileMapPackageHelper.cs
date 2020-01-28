using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MobileMapViewer.Shared.Utilities
{
    internal class MobileMapPackageHelper
    {
        /// <summary>
        /// Extension of the Mobile Map Packages used by ESRI
        /// </summary>
        private const string MmpkExtension = ".mmpk";

        /// <summary>
        /// Gets a list of all the Mobile Map Packages located within the root Local Application Data Folder
        /// </summary>
        /// <returns>List of paths to Mobile Map Packages</returns>
        public static IEnumerable<string> FindAll()
        {
            return Directory
                .GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
                .Where(f => f.Contains(MmpkExtension));
        }

        /// <summary>
        /// Gets a list of all the Mobile Map Packages located within passed path directory
        /// </summary>
        /// <param name="directory">string of full path to directory</param>
        /// <returns>/// <returns>List of paths to Mobile Map Packages</returns></returns>
        public static IEnumerable<string> FindAll(string directory)
        {
            return Directory
                .GetFiles(directory).Where(f => f.Contains(MmpkExtension));
        }

        /// <summary>
        /// Gets the path of where a Mobile Map Package will be Unpacked to
        /// </summary>
        /// <param name="mmpkFilePath">String of full path to the Mobile Map Package</param>
        /// <returns>Directory of where the packed would be unpacked</returns>
        public static string GetUnpackedLocation(string mmpkFilePath)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Path.GetFileName(mmpkFilePath)?.Replace(".mmpk", "") ?? throw new InvalidOperationException());
        }

        /// <summary>
        /// Determines if a Mobile Map Package Has been unpacked
        /// </summary>
        /// <param name="mmpkFilePath">String of full path to the Mobile Map Package</param>
        /// <returns>Has the Mobile Map Package been unpacked</returns>
        public static bool IsUnpacked(string mmpkFilePath)
        {
            return Directory.Exists(GetUnpackedLocation(mmpkFilePath));
        }

        /// <summary>
        /// Safe method to delete Mobile Map Package,  mmpk's can be very large and difficult to side-load this is a safer method to delete a mmpk
        /// </summary>
        /// <param name="mmpkFilePath">String of full path to the Mobile Map Package</param>
        public static void Delete(string mmpkFilePath)
        {
            if (MobileMapPackageHelper.IsUnpacked(mmpkFilePath))
                File.Delete(mmpkFilePath);
            else
                throw new Exception("Attempting to delete a MobileMapPackage that has not been unpacked yet");
        }
    }
}