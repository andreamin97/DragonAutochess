using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

public class GoogleSheetsForUnity : MonoBehaviour
{
    private static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets};
    private static readonly string ApplicationName = "Google Sheets for Unity";
    public string spreadsheetId = "1YNeuMS9ERTmX9XBTKtZ_KExDyFcBGUqZWuIozr68oWw";

    private SheetsService service;

    // Start is called before the first frame update
    private void Start()
    {
        GoogleCredential credential;

        /*using (var stream =
            new FileStream("Assets/credentials.json", FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped((Scopes));
        }*/

        var credentialsTextAsset = Resources.Load<TextAsset>("credentials");
        credential = GoogleCredential.FromJson(credentialsTextAsset.text)
            .CreateScoped(Scopes);

        // Create Google Sheets API service.
        service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
    }

    public void AppendToSheet(string sheet, string range, List<object> values)
    {
        Task.Factory.StartNew(() =>
        {
            var compositeRange = $"{sheet}!{range}";

            var valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>> {values};

            var request =
                service.Spreadsheets.Values.Append(valueRange, spreadsheetId, compositeRange);
            request.ValueInputOption =
                SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            request.Execute();
        });

        //yield return null;
    }
}