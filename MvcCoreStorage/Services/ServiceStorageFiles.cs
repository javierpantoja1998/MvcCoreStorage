using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace MvcCoreStorage.Services
{
    public class ServiceStorageFiles
    {
        //TODO SERVICIO AZURE STORAGE FUNCIONA MEDIANTE CLIENT
        //A PARTIR DE AHI ES CUANDO GENERAMOS LAS FUNCIONALIDADES

        //ESTE OBJETO ES EL DIRECTORIO RAIZ
        private ShareDirectoryClient root;
        public ServiceStorageFiles(IConfiguration configuration)
        {
            string azureKeys =
            configuration.GetValue<string>("AzureKeys:StorageAccount");
            ShareClient shareclient =
            new ShareClient(azureKeys, "ejemplofiles");
            this.root = shareclient.GetRootDirectoryClient();
        }



        //METODO PARA RECUPERAR TODOS LOS FILES Y DEVOLVEMOS SU NAME
        public async Task<List<string>> GetFilesAsync()
        {
            List<string> files = new List<string>();
            //RECORREMOS TODOS LOS FICHEROS DE LA RAIZ
            await foreach (ShareFileItem item in this.root.GetFilesAndDirectoriesAsync())
            {
                files.Add(item.Name);
            }
            return files;
        }



        //METODO PARA LEER UN FICHERO POR SU NOMBRE
        public async Task<string> ReadFilesAsync(string filename)
        {
            //PARA ACCEDER A UN DIRECTORIO NECESITAMOS UN CLIENTE 
            //QUE NOS DIGA QUE DIRECTORIO ES,
            //Y PARA ACCEDER AL FILE NECESITAMOS QUE UN DIRECTORIO NOS LO OFREZCA
            ShareFileClient file = this.root.GetFileClient(filename);
            ShareFileDownloadInfo data = await file.DownloadAsync();
            Stream stream = data.Content;
            string contenido = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                contenido = await reader.ReadToEndAsync();
            }
            return contenido;
        }



        //METODO PARA SUBIR FICHEROS
        public async Task UploadFileAsync(string filename, Stream stream)
        {
            ShareFileClient file = this.root.GetFileClient(filename);
            //CREAMOS EL FICHERO, ES OPCIONAL , PERO SI LE DECIMOS EL TAMAÑO IRA MEJOR
            await file.CreateAsync(stream.Length);
            await file.UploadAsync(stream);
        }



        //METODO PARA ELIMINAR FICHEROS
        public async Task DeleteFilesAsync(string filename)
        {
            ShareFileClient file = this.root.GetFileClient(filename);
            await file.DeleteAsync();
        }
    }
}
