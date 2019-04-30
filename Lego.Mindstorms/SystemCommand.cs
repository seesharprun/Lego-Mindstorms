using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Lego.Mindstorms
{
    /// <summary>
    /// Direct commands for the EV3 brick
    /// </summary>
    public sealed partial class MindstormsClient<T> : IDisposable where T : ICommunication, IDisposable
    {
		/// <summary>
		/// Write a file to the EV3 brick
		/// </summary>
		/// <param name="data">Data to write.</param>
		/// <param name="devicePath">Destination path on the brick.</param>
		/// <returns></returns>
		///	<remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
		public Task WriteFileAsync([ReadOnlyArray]byte[] data, string devicePath)
		{
			return WriteFileAsyncInternal(data, devicePath);
		}

		/// <summary>
		/// Copy a local file to the EV3 brick
		/// </summary>
		/// <param name="localPath">Source path on the computer.</param>
		/// <param name="devicePath">Destination path on the brick.</param>
		/// <returns></returns>
		///	<remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
		public Task CopyFileAsync(string localPath, string devicePath)
		{
			return CopyFileAsyncInternal(localPath, devicePath);
		}

		/// <summary>
		/// Create a directory on the EV3 brick
		/// </summary>
		/// <param name="devicePath">Destination path on the brick.</param>
		/// <returns></returns>
		///	<remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  Destination folders are automatically created if provided in the path.  The path must start with "apps", "prjs", or "tools".</remarks>
		public Task CreateDirectoryAsync(string devicePath)
		{
			return CreateDirectoryAsyncInternal(devicePath);
		}

		/// <summary>
		/// Delete file from the EV3 brick
		/// </summary>
		/// <param name="devicePath">Destination path on the brick.</param>
		/// <returns></returns>
		/// <remarks>devicePath is relative from "lms2012/sys" on the EV3 brick.  The path must start with "apps", "prjs", or "tools".</remarks>
		public Task DeleteFileAsync(string devicePath)
		{
			return DeleteFileAsyncInternal(devicePath);
		}

		internal async Task DeleteFileAsyncInternal(string devicePath)
		{
			Response r = ResponseManager.CreateResponse();
			Command c = new Command(CommandType.SystemReply);
			c.DeleteFile(devicePath);
			await SendCommandAsyncInternal(c);
			if(r.SystemReplyStatus != SystemReplyStatus.Success)
				throw new Exception("Error deleting file: " + r.SystemReplyStatus);
		}

		internal async Task CreateDirectoryAsyncInternal(string devicePath)
		{
			Response r = ResponseManager.CreateResponse();
			Command c = new Command(CommandType.SystemReply);
			c.CreateDirectory(devicePath);
			await SendCommandAsyncInternal(c);
			if(r.SystemReplyStatus != SystemReplyStatus.Success)
				throw new Exception("Error creating directory: " + r.SystemReplyStatus);
		}

		internal async Task CopyFileAsyncInternal(string localPath, string devicePath)
		{
			byte[] data = await GetFileContents(localPath);
			await WriteFileAsyncInternal(data, devicePath);
		}

		internal async Task WriteFileAsyncInternal(byte[] data, string devicePath)
		{
			const int chunkSize = 960;

			Command commandBegin = new Command(CommandType.SystemReply);
			commandBegin.AddOpcode(SystemOpcode.BeginDownload);
			commandBegin.AddRawParameter((uint)data.Length);
			commandBegin.AddRawParameter(devicePath);

			await SendCommandAsyncInternal(commandBegin);
			if(commandBegin.Response.SystemReplyStatus != SystemReplyStatus.Success)
				throw new Exception("Could not begin file save: " + commandBegin.Response.SystemReplyStatus);

			byte handle = commandBegin.Response.Data[0];
			int sizeSent = 0;

			while(sizeSent < data.Length)
			{
				Command commandContinue = new Command(CommandType.SystemReply);
				commandContinue.AddOpcode(SystemOpcode.ContinueDownload);
				commandContinue.AddRawParameter(handle);
				int sizeToSend = Math.Min(chunkSize, data.Length - sizeSent);
				commandContinue.AddRawParameter(data, sizeSent, sizeToSend);
				sizeSent += sizeToSend;

				await SendCommandAsyncInternal(commandContinue);
				if(commandContinue.Response.SystemReplyStatus != SystemReplyStatus.Success &&
					(commandContinue.Response.SystemReplyStatus != SystemReplyStatus.EndOfFile && sizeSent == data.Length))
					throw new Exception("Error saving file: " + commandContinue.Response.SystemReplyStatus);
			}

			//Command commandClose = new Command(CommandType.SystemReply);
			//commandClose.AddOpcode(SystemOpcode.CloseFileHandle);
			//commandClose.AddRawParameter(handle);
			//await SendCommandAsyncInternal(commandClose);
			//if(commandClose.Response.SystemReplyStatus != SystemReplyStatus.Success)
			//	throw new Exception("Could not close handle: " + commandClose.Response.SystemReplyStatus);
		}
        private Task<byte[]> GetFileContents(string localPath)
		{
			return Task.FromResult(File.ReadAllBytes(localPath));
		}
	}
}
