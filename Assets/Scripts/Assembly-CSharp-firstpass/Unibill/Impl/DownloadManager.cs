using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Uniject;
using UnityEngine;

namespace Unibill.Impl
{
	public class DownloadManager
	{
		[CompilerGenerated]
		private sealed class _003Cdownload_003Ec__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal string bundleId;

			internal string _003CdownloadToken_003E__0;

			internal Dictionary<string, string> _003Cparameters_003E__1;

			internal IHTTPRequest _003Cresponse_003E__2;

			internal Dictionary<string, object> _003CdownloadTokenHash_003E__3;

			internal bool _003Csuccess_003E__4;

			internal string _003CerrorString_003E__5;

			internal string _003Cversion_003E__6;

			internal Dictionary<string, string> _003Cheaders_003E__7;

			internal IHTTPRequest _003Cresponse_003E__8;

			internal string _003Cerror_003E__9;

			internal string _003CcontentRange_003E__10;

			internal long _003CcontentLength_003E__11;

			internal PurchasableItem _003CdownloadingItem_003E__12;

			internal FileStream _003C_0024s_30_003E__13;

			internal long _003CrangeStart_003E__14;

			internal long _003CrangeEnd_003E__15;

			internal int _003ClastProgress_003E__16;

			internal string _003Cheader_003E__17;

			internal IHTTPRequest _003Cresponse_003E__18;

			internal int _003Cprogress_003E__19;

			internal int _0024PC;

			internal object _0024current;

			internal string _003C_0024_003EbundleId;

			internal DownloadManager _003C_003Ef__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _0024current;
				}
			}

			public bool MoveNext()
			{
				//Discarded unreachable code: IL_00ff, IL_0a92
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					Directory.CreateDirectory(_003C_003Ef__this.getDataPath(bundleId));
					if (!File.Exists(_003C_003Ef__this.getZipPath(bundleId)))
					{
						_003C_003Ef__this.logger.Log(bundleId);
						_003CdownloadToken_003E__0 = string.Empty;
						_003Cparameters_003E__1 = new Dictionary<string, string>();
						try
						{
							_003Cparameters_003E__1.Add("receipt", _003C_003Ef__this.receiptStore.getItemReceiptForFilebundle(bundleId));
						}
						catch (ArgumentException)
						{
							_003C_003Ef__this.onDownloadFailedPermanently(bundleId, string.Format("Bundle {0} no longer defined in inventory!", bundleId));
							goto default;
						}
						_003Cparameters_003E__1.Add("bundleId", bundleId);
						_003Cparameters_003E__1.Add("platform", _003C_003Ef__this.platform.ToString());
						_003Cparameters_003E__1.Add("appSecret", _003C_003Ef__this.appSecret);
						_003Cparameters_003E__1.Add("version", _003C_003Ef__this.getVersionToDownload(bundleId));
						_003Cparameters_003E__1.Add("unibillVersion", "1.7.5");
						_0024current = _003C_003Ef__this.fetcher.doPost("http://cdn.unibiller.com/download_token", _003Cparameters_003E__1);
						_0024PC = 1;
						break;
					}
					goto IL_09d0;
				case 1u:
					_003Cresponse_003E__2 = _003C_003Ef__this.fetcher.getResponse();
					if (!string.IsNullOrEmpty(_003Cresponse_003E__2.error))
					{
						_003C_003Ef__this.logger.Log("Error downloading content: {0}. Unibill will retry later.", _003Cresponse_003E__2.error);
						_0024current = _003C_003Ef__this.getRandomSleep();
						_0024PC = 2;
						break;
					}
					_003CdownloadTokenHash_003E__3 = (Dictionary<string, object>)MiniJSON.jsonDecode(_003Cresponse_003E__2.contentString);
					if (_003CdownloadTokenHash_003E__3 == null)
					{
						_003C_003Ef__this.logger.Log("Error fetching download token. Unibill will retry later.");
						_0024current = _003C_003Ef__this.getRandomSleep();
						_0024PC = 3;
						break;
					}
					_003Csuccess_003E__4 = bool.Parse(_003CdownloadTokenHash_003E__3["success"].ToString());
					if (!_003Csuccess_003E__4)
					{
						_003C_003Ef__this.logger.LogError("Error downloading bundle {0}. Download abandoned.", bundleId);
						_003CerrorString_003E__5 = string.Empty;
						if (_003CdownloadTokenHash_003E__3.ContainsKey("error"))
						{
							_003CerrorString_003E__5 = _003CdownloadTokenHash_003E__3["error"].ToString();
							_003C_003Ef__this.logger.LogError(_003CerrorString_003E__5);
						}
						_003C_003Ef__this.onDownloadFailedPermanently(bundleId, _003CerrorString_003E__5);
						goto default;
					}
					if (!_003CdownloadTokenHash_003E__3.ContainsKey("url"))
					{
						_003C_003Ef__this.logger.LogError("Error fetching download token. Missing URL. Will retry");
						_0024current = _003C_003Ef__this.getRandomSleep();
						_0024PC = 4;
						break;
					}
					_003CdownloadToken_003E__0 = _003CdownloadTokenHash_003E__3["url"].ToString();
					if (!_003CdownloadTokenHash_003E__3.ContainsKey("version"))
					{
						_003C_003Ef__this.logger.LogError("Error fetching download token. Missing version. Will retry");
						_0024current = _003C_003Ef__this.getRandomSleep();
						_0024PC = 5;
						break;
					}
					_003Cversion_003E__6 = _003CdownloadTokenHash_003E__3["version"].ToString();
					_003C_003Ef__this.saveVersion(bundleId, _003Cversion_003E__6);
					_003Cheaders_003E__7 = new Dictionary<string, string>();
					_003Cheaders_003E__7["If-Modified-Since"] = "Tue, 1 Jan 1980 00:00:00 GMT";
					_003Cheaders_003E__7["If-None-Match"] = "notanetag";
					_003Cheaders_003E__7["Range"] = "bytes=0-1";
					_0024current = _003C_003Ef__this.fetcher.doGet(_003CdownloadToken_003E__0, _003Cheaders_003E__7);
					_0024PC = 6;
					break;
				case 6u:
					_003Cresponse_003E__8 = _003C_003Ef__this.fetcher.getResponse();
					if (_003C_003Ef__this.isContentNotFound(_003Cresponse_003E__8))
					{
						_003Cerror_003E__9 = string.Format("404 - Downloadable Content missing for bundle {0}!", bundleId);
						_003C_003Ef__this.logger.LogError(_003Cerror_003E__9);
						_003C_003Ef__this.onDownloadFailedPermanently(bundleId, _003Cerror_003E__9);
						goto default;
					}
					if (!_003Cresponse_003E__8.responseHeaders.ContainsKey("CONTENT-RANGE"))
					{
						_003C_003Ef__this.logger.LogError("Malformed server response. Missing content-range");
						_003C_003Ef__this.logger.LogError(_003Cresponse_003E__8.error);
						_0024current = _003C_003Ef__this.getRandomSleep();
						_0024PC = 7;
						break;
					}
					_003CcontentRange_003E__10 = _003Cresponse_003E__8.responseHeaders["CONTENT-RANGE"].ToString();
					_003CcontentLength_003E__11 = long.Parse(_003CcontentRange_003E__10.Split(new char[1] { '/' }, 2)[1]);
					_003CdownloadingItem_003E__12 = _003C_003Ef__this.receiptStore.getItemFromFileBundle(bundleId);
					_003C_0024s_30_003E__13 = (_003C_003Ef__this.fileStream = _003C_003Ef__this.openDownload(bundleId));
					num = 4294967293u;
					goto case 8u;
				case 8u:
				case 9u:
				case 10u:
				case 11u:
					try
					{
						switch (num)
						{
						default:
							_003CrangeStart_003E__14 = _003C_003Ef__this.fileStream.Length;
							if (_003CrangeStart_003E__14 > 0)
							{
								_003C_003Ef__this.fileStream.Seek(0L, SeekOrigin.End);
							}
							_003CrangeEnd_003E__15 = Math.Min(_003CrangeStart_003E__14 + _003C_003Ef__this.bufferSize, _003CcontentLength_003E__11);
							_003ClastProgress_003E__16 = -1;
							break;
						case 8u:
							_003Cresponse_003E__18 = _003C_003Ef__this.fetcher.getResponse();
							if (!string.IsNullOrEmpty(_003Cresponse_003E__18.error))
							{
								_003C_003Ef__this.logger.LogError("Error downloading content. Will retry.");
								_003C_003Ef__this.logger.LogError(_003Cresponse_003E__18.error);
								_0024current = _003C_003Ef__this.getRandomSleep();
								_0024PC = 9;
								flag = true;
								goto end_IL_0011;
							}
							_003Cprogress_003E__19 = (int)((float)_003CrangeEnd_003E__15 / (float)_003CcontentLength_003E__11 * 100f);
							_003Cprogress_003E__19 = Math.Min(99, _003Cprogress_003E__19);
							if (_003C_003Ef__this.onDownloadProgressedEvent != null && _003ClastProgress_003E__16 != _003Cprogress_003E__19)
							{
								_003C_003Ef__this.onDownloadProgressedEvent(_003CdownloadingItem_003E__12, _003Cprogress_003E__19);
								_003ClastProgress_003E__16 = _003Cprogress_003E__19;
							}
							if (_003Cresponse_003E__18.bytes.Length > _003C_003Ef__this.BUFFER.Length)
							{
								_003C_003Ef__this.logger.LogError("Malformed content. Unexpected length. Will retry.");
								_0024current = _003C_003Ef__this.getRandomSleep();
								_0024PC = 10;
								flag = true;
								goto end_IL_0011;
							}
							Buffer.BlockCopy(_003Cresponse_003E__18.bytes, 0, _003C_003Ef__this.BUFFER, 0, _003Cresponse_003E__18.bytes.Length);
							_003C_003Ef__this.bytesReceived = _003Cresponse_003E__18.bytes.Length;
							_003C_003Ef__this.DATA_FLUSHED = false;
							_003C_003Ef__this.DATA_READY.Set();
							goto case 11u;
						case 9u:
							goto end_IL_0637;
						case 10u:
							goto end_IL_0637;
						case 11u:
							if (!_003C_003Ef__this.DATA_FLUSHED)
							{
								_0024current = _003C_003Ef__this.waiter;
								_0024PC = 11;
								flag = true;
								goto end_IL_0011;
							}
							_003CrangeStart_003E__14 = _003CrangeEnd_003E__15 + 1;
							_003CrangeEnd_003E__15 = _003CrangeStart_003E__14 + _003C_003Ef__this.bufferSize;
							_003CrangeEnd_003E__15 = Math.Min(_003CrangeEnd_003E__15, _003CcontentLength_003E__11);
							break;
						}
						if (_003CrangeStart_003E__14 < _003CrangeEnd_003E__15)
						{
							_003Cheader_003E__17 = string.Format("bytes={0}-{1}", _003CrangeStart_003E__14, _003CrangeEnd_003E__15);
							_003Cheaders_003E__7["Range"] = _003Cheader_003E__17;
							_0024current = _003C_003Ef__this.fetcher.doGet(_003CdownloadToken_003E__0, _003Cheaders_003E__7);
							_0024PC = 8;
							flag = true;
							break;
						}
						goto IL_0993;
						end_IL_0637:;
					}
					finally
					{
						if (!flag && _003C_0024s_30_003E__13 != null)
						{
							((IDisposable)_003C_0024s_30_003E__13).Dispose();
						}
					}
					goto default;
				case 12u:
					if (!_003C_003Ef__this.UNPACK_FINISHED)
					{
						_0024current = _003C_003Ef__this.waiter;
						_0024PC = 12;
						break;
					}
					_003C_003Ef__this.removeDownloadFromQueues(bundleId);
					if (_003C_003Ef__this.onDownloadCompletedEvent != null)
					{
						_003C_003Ef__this.onDownloadCompletedEvent(_003C_003Ef__this.receiptStore.getItemFromFileBundle(bundleId), _003C_003Ef__this.getContentPath(bundleId));
					}
					_0024PC = -1;
					goto default;
				default:
					{
						return false;
					}
					IL_09d0:
					_003C_003Ef__this.UNPACK_FINISHED = false;
					_003C_003Ef__this.util.RunOnThreadPool(_003C_003Em__C);
					goto case 12u;
					IL_0993:
					File.Move(_003C_003Ef__this.getPartialPath(bundleId), _003C_003Ef__this.getZipPath(bundleId));
					File.Delete(_003C_003Ef__this.getVersionPath(bundleId));
					goto IL_09d0;
					end_IL_0011:
					break;
				}
				return true;
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				switch (num)
				{
				case 8u:
				case 9u:
				case 10u:
				case 11u:
					try
					{
						break;
					}
					finally
					{
						if (_003C_0024s_30_003E__13 != null)
						{
							((IDisposable)_003C_0024s_30_003E__13).Dispose();
						}
					}
				case 0u:
				case 1u:
				case 2u:
				case 3u:
				case 4u:
				case 5u:
				case 6u:
				case 7u:
				case 12u:
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			internal void _003C_003Em__C()
			{
				_003C_003Ef__this.Unpack(bundleId);
			}
		}

		[CompilerGenerated]
		private sealed class _003ConDownloadFailedPermanently_003Ec__AnonStorey7
		{
			internal string bundleId;

			internal string error;

			internal DownloadManager _003C_003Ef__this;

			internal void _003C_003Em__B()
			{
				_003C_003Ef__this.removeDownloadFromQueues(bundleId);
				if (_003C_003Ef__this.onDownloadFailedEvent != null)
				{
					try
					{
						_003C_003Ef__this.onDownloadFailedEvent(_003C_003Ef__this.receiptStore.getItemFromFileBundle(bundleId), error);
					}
					catch (ArgumentException)
					{
						_003C_003Ef__this.onDownloadFailedEvent(null, error);
					}
				}
			}
		}

		private const string DOWNLOAD_TOKEN_URL = "http://cdn.unibiller.com/download_token";

		private const string SCHEDULED_DOWNLOADS_KEY = "com.outlinegames.unibill.scheduled_downloads";

		private const int DEFAULT_BUFFER_SIZE = 2000000;

		private IReceiptStore receiptStore;

		private IUtil util;

		private IStorage storage;

		private IURLFetcher fetcher;

		private Uniject.ILogger logger;

		private volatile string persistentDataPath;

		private List<string> scheduledDownloads = new List<string>();

		private int bufferSize = 2000000;

		private byte[] BUFFER = new byte[2200000];

		private AutoResetEvent DATA_READY = new AutoResetEvent(false);

		private volatile bool UNPACK_FINISHED;

		private volatile bool DATA_FLUSHED;

		private volatile FileStream fileStream;

		private volatile int bytesReceived;

		private BillingPlatform platform;

		private string appSecret;

		private WaitForFixedUpdate waiter = new WaitForFixedUpdate();

		private System.Random rand = new System.Random();

		public event Action<PurchasableItem, string> onDownloadCompletedEvent;

		public event Action<PurchasableItem, string> onDownloadFailedEvent;

		public event Action<PurchasableItem, int> onDownloadProgressedEvent;

		public DownloadManager(IReceiptStore receiptStore, IUtil util, IStorage storage, IURLFetcher fetcher, Uniject.ILogger logger, BillingPlatform platform, string appSecret)
		{
			this.receiptStore = receiptStore;
			this.util = util;
			this.storage = storage;
			this.fetcher = fetcher;
			this.logger = logger;
			this.platform = platform;
			this.appSecret = appSecret;
			scheduledDownloads = deserialiseDownloads();
			persistentDataPath = util.persistentDataPath;
			Thread thread = new Thread(DownloadFlusher);
			thread.IsBackground = true;
			thread.Start();
		}

		public void setBufferSize(int size)
		{
			bufferSize = size;
		}

		public void downloadContentFor(PurchasableItem item)
		{
			if (!item.hasDownloadableContent)
			{
				if (this.onDownloadFailedEvent != null)
				{
					this.onDownloadFailedEvent(item, "The item has no downloadable content");
				}
			}
			else if (isDownloaded(item.downloadableContentId))
			{
				this.onDownloadCompletedEvent(item, getContentPath(item.downloadableContentId));
			}
			else if (!receiptStore.hasItemReceiptForFilebundle(item.downloadableContentId))
			{
				if (this.onDownloadFailedEvent != null)
				{
					this.onDownloadFailedEvent(item, "The item is not owned");
				}
			}
			else if (!scheduledDownloads.Contains(item.downloadableContentId))
			{
				scheduledDownloads.Add(item.downloadableContentId);
				serialiseDownloads();
			}
		}

		public bool isDownloadScheduled(string bundleId)
		{
			return scheduledDownloads.Contains(bundleId);
		}

		public IEnumerator checkDownloads()
		{
			for (int t = 0; t < scheduledDownloads.Count; t++)
			{
				string scheduledDownload = scheduledDownloads[t];
				yield return util.InitiateCoroutine(download(scheduledDownload.ToString()));
			}
		}

		public IEnumerator monitorDownloads()
		{
			while (true)
			{
				if (scheduledDownloads.Count > 0)
				{
					yield return util.InitiateCoroutine(download(scheduledDownloads[0]));
				}
				else
				{
					yield return waiter;
				}
			}
		}

		public int getQueueSize()
		{
			return scheduledDownloads.Count;
		}

		private List<string> deserialiseDownloads()
		{
			List<object> list = storage.GetString("com.outlinegames.unibill.scheduled_downloads", "[]").arrayListFromJson();
			List<string> list2 = new List<string>();
			if (list != null)
			{
				foreach (object item in list)
				{
					list2.Add(item.ToString());
				}
				return list2;
			}
			return list2;
		}

		private void serialiseDownloads()
		{
			List<object> list = new List<object>();
			foreach (string scheduledDownload in scheduledDownloads)
			{
				list.Add(scheduledDownload);
			}
			storage.SetString("com.outlinegames.unibill.scheduled_downloads", MiniJSON.jsonEncode(list));
		}

		private IEnumerator download(string bundleId)
		{
			Directory.CreateDirectory(getDataPath(bundleId));
			if (!File.Exists(getZipPath(bundleId)))
			{
				logger.Log(bundleId);
				string downloadToken = string.Empty;
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				try
				{
					parameters.Add("receipt", receiptStore.getItemReceiptForFilebundle(bundleId));
				}
				catch (ArgumentException)
				{
					onDownloadFailedPermanently(bundleId, string.Format("Bundle {0} no longer defined in inventory!", bundleId));
					yield break;
				}
				parameters.Add("bundleId", bundleId);
				parameters.Add("platform", platform.ToString());
				parameters.Add("appSecret", appSecret);
				parameters.Add("version", getVersionToDownload(bundleId));
				parameters.Add("unibillVersion", "1.7.5");
				yield return fetcher.doPost("http://cdn.unibiller.com/download_token", parameters);
				IHTTPRequest response2 = fetcher.getResponse();
				if (!string.IsNullOrEmpty(response2.error))
				{
					logger.Log("Error downloading content: {0}. Unibill will retry later.", response2.error);
					yield return getRandomSleep();
					yield break;
				}
				Dictionary<string, object> downloadTokenHash = (Dictionary<string, object>)MiniJSON.jsonDecode(response2.contentString);
				if (downloadTokenHash == null)
				{
					logger.Log("Error fetching download token. Unibill will retry later.");
					yield return getRandomSleep();
					yield break;
				}
				if (!bool.Parse(downloadTokenHash["success"].ToString()))
				{
					logger.LogError("Error downloading bundle {0}. Download abandoned.", bundleId);
					string errorString = string.Empty;
					if (downloadTokenHash.ContainsKey("error"))
					{
						errorString = downloadTokenHash["error"].ToString();
						logger.LogError(errorString);
					}
					onDownloadFailedPermanently(bundleId, errorString);
					yield break;
				}
				if (!downloadTokenHash.ContainsKey("url"))
				{
					logger.LogError("Error fetching download token. Missing URL. Will retry");
					yield return getRandomSleep();
					yield break;
				}
				downloadToken = downloadTokenHash["url"].ToString();
				if (!downloadTokenHash.ContainsKey("version"))
				{
					logger.LogError("Error fetching download token. Missing version. Will retry");
					yield return getRandomSleep();
					yield break;
				}
				string version = downloadTokenHash["version"].ToString();
				saveVersion(bundleId, version);
				Dictionary<string, string> headers = new Dictionary<string, string>();
				headers["If-Modified-Since"] = "Tue, 1 Jan 1980 00:00:00 GMT";
				headers["If-None-Match"] = "notanetag";
				headers["Range"] = "bytes=0-1";
				yield return fetcher.doGet(downloadToken, headers);
				IHTTPRequest response3 = fetcher.getResponse();
				if (isContentNotFound(response3))
				{
					string error = string.Format("404 - Downloadable Content missing for bundle {0}!", bundleId);
					logger.LogError(error);
					onDownloadFailedPermanently(bundleId, error);
					yield break;
				}
				if (!response3.responseHeaders.ContainsKey("CONTENT-RANGE"))
				{
					logger.LogError("Malformed server response. Missing content-range");
					logger.LogError(response3.error);
					yield return getRandomSleep();
					yield break;
				}
				string contentRange = response3.responseHeaders["CONTENT-RANGE"].ToString();
				long contentLength = long.Parse(contentRange.Split(new char[1] { '/' }, 2)[1]);
				PurchasableItem downloadingItem = receiptStore.getItemFromFileBundle(bundleId);
				using (this.fileStream = openDownload(bundleId))
				{
					long rangeStart = this.fileStream.Length;
					if (rangeStart > 0)
					{
						this.fileStream.Seek(0L, SeekOrigin.End);
					}
					long rangeEnd2 = Math.Min(rangeStart + bufferSize, contentLength);
					int lastProgress = -1;
					while (rangeStart < rangeEnd2)
					{
						string header = string.Format("bytes={0}-{1}", rangeStart, rangeEnd2);
						headers["Range"] = header;
						yield return fetcher.doGet(downloadToken, headers);
						IHTTPRequest response = fetcher.getResponse();
						if (!string.IsNullOrEmpty(response.error))
						{
							logger.LogError("Error downloading content. Will retry.");
							logger.LogError(response.error);
							yield return getRandomSleep();
							yield break;
						}
						int progress2 = (int)((float)rangeEnd2 / (float)contentLength * 100f);
						progress2 = Math.Min(99, progress2);
						if (this.onDownloadProgressedEvent != null && lastProgress != progress2)
						{
							this.onDownloadProgressedEvent(downloadingItem, progress2);
							lastProgress = progress2;
						}
						if (response.bytes.Length > BUFFER.Length)
						{
							logger.LogError("Malformed content. Unexpected length. Will retry.");
							yield return getRandomSleep();
							yield break;
						}
						Buffer.BlockCopy(response.bytes, 0, BUFFER, 0, response.bytes.Length);
						bytesReceived = response.bytes.Length;
						DATA_FLUSHED = false;
						DATA_READY.Set();
						while (!DATA_FLUSHED)
						{
							yield return waiter;
						}
						rangeStart = rangeEnd2 + 1;
						rangeEnd2 = rangeStart + bufferSize;
						rangeEnd2 = Math.Min(rangeEnd2, contentLength);
					}
				}
				File.Move(getPartialPath(bundleId), getZipPath(bundleId));
				File.Delete(getVersionPath(bundleId));
			}
			UNPACK_FINISHED = false;
//			util.RunOnThreadPool(((_003Cdownload_003Ec__Iterator3)this)._003C_003Em__C);
			while (!UNPACK_FINISHED)
			{
				yield return waiter;
			}
			removeDownloadFromQueues(bundleId);
			if (this.onDownloadCompletedEvent != null)
			{
				this.onDownloadCompletedEvent(receiptStore.getItemFromFileBundle(bundleId), getContentPath(bundleId));
			}
		}

		private bool isContentNotFound(IHTTPRequest request)
		{
			foreach (KeyValuePair<string, string> responseHeader in request.responseHeaders)
			{
				if (responseHeader.Value.ToUpper().Contains("404 NOT FOUND"))
				{
					return true;
				}
			}
			if (request.error != null)
			{
				return request.error.Contains("404");
			}
			return false;
		}

		private void Unpack(string bundleId)
		{
			try
			{
				string zipPath = getZipPath(bundleId);
				if (!File.Exists(zipPath))
				{
					logger.LogError("No download found: " + zipPath);
					return;
				}
				logger.Log("Verifying download...");
				if (!verifyDownload(zipPath))
				{
					logger.LogError("Download failed integrity check. Deleting...");
					Directory.Delete(getDataPath(bundleId), true);
					return;
				}
				logger.Log("Download verified.");
				logger.Log("Unpacking");
				DeleteIfExists(getUnpackPath(bundleId));
				Directory.CreateDirectory(getUnpackPath(bundleId));
				using (FileStream stream = new FileStream(getZipPath(bundleId), FileMode.Open))
				{
					ZipUtils.decompress(stream, getUnpackPath(bundleId));
				}
				logger.Log("Unpack complete");
				DeleteIfExists(getContentPath(bundleId));
				Directory.Move(getUnpackPath(bundleId), getContentPath(bundleId));
				File.Delete(getZipPath(bundleId));
			}
			catch (IOException ex)
			{
				logger.LogError(ex.Message);
				onDownloadFailedPermanently(bundleId, ex.Message);
			}
			catch (Exception ex2)
			{
				logger.LogError(ex2.Message);
				logger.LogError(ex2.StackTrace);
				onDownloadFailedPermanently(bundleId, ex2.Message);
			}
			finally
			{
				UNPACK_FINISHED = true;
			}
		}

		private void DeleteIfExists(string folder)
		{
			if (Directory.Exists(folder))
			{
				Directory.Delete(folder, true);
			}
		}

		private void onDownloadFailedPermanently(string bundleId, string error)
		{
			_003ConDownloadFailedPermanently_003Ec__AnonStorey7 _003ConDownloadFailedPermanently_003Ec__AnonStorey = new _003ConDownloadFailedPermanently_003Ec__AnonStorey7();
			_003ConDownloadFailedPermanently_003Ec__AnonStorey.bundleId = bundleId;
			_003ConDownloadFailedPermanently_003Ec__AnonStorey.error = error;
			_003ConDownloadFailedPermanently_003Ec__AnonStorey._003C_003Ef__this = this;
			util.RunOnMainThread(_003ConDownloadFailedPermanently_003Ec__AnonStorey._003C_003Em__B);
		}

		private void removeDownloadFromQueues(string bundleId)
		{
			scheduledDownloads.Remove(bundleId);
			serialiseDownloads();
		}

		private bool verifyDownload(string filepath)
		{
			//Discarded unreachable code: IL_0014, IL_0026, IL_0033
			try
			{
				using (ZipFile zipFile = new ZipFile(filepath))
				{
					return zipFile.TestArchive(true);
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		private void DownloadFlusher()
		{
			while (true)
			{
				DATA_READY.WaitOne();
				fileStream.Write(BUFFER, 0, bytesReceived);
				DATA_FLUSHED = true;
			}
		}

		private byte[] decodeBase64String(string s)
		{
			return Convert.FromBase64String(s);
		}

		private FileStream openDownload(string bundleId)
		{
			return new FileStream(getPartialPath(bundleId), FileMode.OpenOrCreate);
		}

		public string getContentPath(string bundleId)
		{
			return Path.Combine(getDataPath(bundleId), "content");
		}

		private string getUnpackPath(string bundleId)
		{
			return Path.Combine(getDataPath(bundleId), "unpack");
		}

		private string getZipPath(string bundleId)
		{
			return Path.Combine(getDataPath(bundleId), "download.zip");
		}

		private string getPartialPath(string bundleId)
		{
			return Path.Combine(getDataPath(bundleId), "download.partial");
		}

		private void saveVersion(string bundleId, string version)
		{
			Util.WriteAllText(getVersionPath(bundleId), version);
		}

		private string getVersionToDownload(string bundleId)
		{
			string versionPath = getVersionPath(bundleId);
			if (File.Exists(versionPath))
			{
				string text = Util.ReadAllText(versionPath);
				long result;
				if (long.TryParse(text, out result))
				{
					return text;
				}
			}
			return "*";
		}

		private string getVersionPath(string bundleId)
		{
			return Path.Combine(getDataPath(bundleId), "download.version");
		}

		private string getRootContentPath()
		{
			return Path.Combine(persistentDataPath, "unibill-content");
		}

		public string getDataPath(string bundleId)
		{
			return Path.Combine(getRootContentPath(), bundleId);
		}

		public bool isDownloaded(string bundleId)
		{
			return Directory.Exists(getContentPath(bundleId));
		}

		public void deleteContent(string bundleId)
		{
			if (isDownloadScheduled(bundleId))
			{
				logger.LogError("Bundle id {0} is still downloading", bundleId);
			}
			else if (!isDownloaded(bundleId))
			{
				logger.LogError("Bundle id {0} is not downloaded", bundleId);
			}
			else
			{
				Directory.Delete(getDataPath(bundleId), true);
			}
		}

		private object getRandomSleep()
		{
			int num = 30 + rand.Next(30);
			logger.Log("Backing off for {0} seconds", num);
			return util.getWaitForSeconds(num);
		}
	}
}
