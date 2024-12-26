using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;

namespace BHDReader;

/// <summary>
/// These RSA functions are copy-pasted straight from BinderTool and modified slightly for this library. Thank you Atvaark!
/// </summary>
internal static class CryptoUtil {
    /// <summary>
    /// Decrypts a file with a provided decryption key.
    /// </summary>
    /// <param name="file">An encrypted file</param>
    /// <param name="key">The RSA key in PEM format</param>
    /// <param name="token"></param>
    /// <exception cref="ArgumentNullException">When the argument filePath is null</exception>
    /// <exception cref="ArgumentNullException">When the argument keyPath is null</exception>
    /// <returns>A memory stream with the decrypted file</returns>
    public static MemoryStream DecryptRsa(byte[] file, string key, CancellationToken? token = null) {
        if (file == null) {
            throw new ArgumentNullException(nameof(file));
        }

        if (key == null) {
            throw new ArgumentNullException(nameof(key));
        }

        AsymmetricKeyParameter keyParameter = getKeyOrDefault(key) ?? throw new InvalidOperationException();
        RsaEngine engine = new();
        engine.Init(false, keyParameter);

        MemoryStream outputStream = new();
        using (Stream inputStream = new MemoryStream(file)) {
            CancellationToken cancellationToken = token ?? CancellationToken.None;
            int inputBlockSize = engine.GetInputBlockSize();
            int outputBlockSize = engine.GetOutputBlockSize();
            byte[] inputBlock = new byte[inputBlockSize];
            while (inputStream.Read(inputBlock, 0, inputBlock.Length) > 0) {
                cancellationToken.ThrowIfCancellationRequested();
                ;
                byte[] outputBlock = engine.ProcessBlock(inputBlock, 0, inputBlockSize);

                int requiredPadding = outputBlockSize - outputBlock.Length;
                if (requiredPadding > 0) {
                    byte[] paddedOutputBlock = new byte[outputBlockSize];
                    outputBlock.CopyTo(paddedOutputBlock, requiredPadding);
                    outputBlock = paddedOutputBlock;
                }

                outputStream.Write(outputBlock, 0, outputBlock.Length);
            }
        }

        outputStream.Seek(0, SeekOrigin.Begin);
        return outputStream;
    }

    private static AsymmetricKeyParameter? getKeyOrDefault(string key) {
        try {
            PemReader pemReader = new(new StringReader(key));
            return (AsymmetricKeyParameter)pemReader.ReadObject();
        }
        catch {
            return null;
        }
    }
}
