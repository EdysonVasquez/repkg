using System.IO;
using System.Text;
using RePKG.Application.Exceptions;
using RePKG.Core.Texture;

namespace RePKG.Application.Texture
{
    public class TexWriter : ITexWriter
    {
        private readonly ITexHeaderWriter _texHeaderWriter;
        private readonly ITexImageContainerWriter _texImageContainerWriter;

        public TexWriter(
            ITexHeaderWriter texHeaderWriter,
            ITexImageContainerWriter texImageContainerWriter)
        {
            _texHeaderWriter = texHeaderWriter;
            _texImageContainerWriter = texImageContainerWriter;
        }

        public void WriteToStream(Tex tex, Stream stream)
        {
            if (tex.Magic1 != "TEXV0005")
                throw new UnknownTexHeaderMagicException(nameof(tex.Magic1), tex.Magic1);
            
            if (tex.Magic2 != "TEXI0001")
                throw new UnknownTexHeaderMagicException(nameof(tex.Magic2), tex.Magic2);
            
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.WriteNString(tex.Magic1);
                writer.WriteNString(tex.Magic2);
            }
            
            _texHeaderWriter.WriteToStream(tex.Header, stream);
            _texImageContainerWriter.WriteToStream(tex.ImagesContainer, stream);
            
            _texImageContainerWriter.WriteImagesToStream(tex, stream);
        }
    }
}