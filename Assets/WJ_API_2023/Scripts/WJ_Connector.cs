using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using WjChallenge;

/// <summary>
//* My Game Code : E28
//* My X-API-KEY : 2fMFJkLhGTGKc7G0kFe08hfSa3U1VStHcXCtuNzAFzw
/// </summary>

public class WJ_Connector : MonoBehaviour
{
    [Header("My Info")]
    public string strGameCD;        //게임코드
    public string strGameKey;       //게임키(Api Key)

    [SerializeField] private string strAuthorization;
    [SerializeField] private string strMBR_ID;
    private string strDeviceNm;     //디바이스 이름
    private string strOsScnCd;      //OS
    private string strGameVer;      //게임 버전

    #region StoredData
    [HideInInspector]
    public DN_Response                  cDiagnotics     = null; //진단 - 현재 풀고있는 진단평가 문제
    [HideInInspector]
    public Response_Learning_Setting    cLearnSet       = null; //학습 - 학습 문항 요청 시 받아온 학습 데이터
    [HideInInspector]
    public Response_Learning_Progress   cLearnProg      = null; //학습 - 학습 완료 시 받아온 결과
    [HideInInspector]
    public List<Learning_MyAnsr>        cMyAnsrs        = null;
    [HideInInspector]
    public Request_DN_Progress          result          = null;
    [HideInInspector]
    public string                       qstCransr       = "";
    #endregion

    #region UNITYEVENT (CALLBACK)
    [HideInInspector]
    public UnityEvent onGetDiagnosis;
    [HideInInspector]
    public UnityEvent onGetLearning;
    #endregion

    private void Awake() {
        if (SystemInfo.deviceType == DeviceType.Desktop) 
            strDeviceNm = "PC";
        else 
            strDeviceNm = SystemInfo.deviceModel;

        strOsScnCd      = SystemInfo.operatingSystem;
        strGameVer      = Application.version;

        if (strOsScnCd.Length >= 15) strOsScnCd = strOsScnCd.Substring(0, 14);

        //
        if(DB.Dt.MyAuthorization != "") strAuthorization = DB.Dt.MyAuthorization;
        //
        if(DB.Dt.MyMBR_ID != "") strMBR_ID = DB.Dt.MyMBR_ID;
        else Make_MBR_ID();
    }

    //현재 시간을 기준으로 MBR ID 생성
    private void Make_MBR_ID() {
        DateTime dt = DateTime.Now;
        strMBR_ID = string.Format("{0}{1:0000}{2:00}{3:00}{4:00}{5:00}{6:00}{7:000}", strGameCD, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        DB.Dt.MyMBR_ID = strMBR_ID;
    }

    #region Function Progress

    /// <summary>
    /// 진단평가 첫 실행 시 서버와 통신
    /// </summary>
    private IEnumerator Send_Diagnosis(int level) {
        Debug.Log($"WJ_Connector:: Send_Diagnosis(level= {level}):: ");
        Request_DN_Setting request = new Request_DN_Setting();

        request.gameCd = strGameCD;
        request.mbrId = strMBR_ID;
        request.deviceNm = strDeviceNm;
        request.gameVer = strGameVer;
        request.osScnCd = strOsScnCd;
        request.langCd = "KO";
        request.timeZone = TimeZoneInfo.Local.BaseUtcOffset.Hours;

        switch (level){
            case 0: request.bgnLvl = "A"; break;
            case 1: request.bgnLvl = "B"; break;
            case 2: request.bgnLvl = "C"; break;
            case 3: request.bgnLvl = "D"; break;
            default: request.bgnLvl = "A"; break;
        }

        Debug.Log("<color=yellow>Send_Diagnosis:: Request_DN_Setting</color>"
            + "\n gameCd= " + request.gameCd + "\n mbrId= " + request.mbrId + "\n deviceNm= " + request.deviceNm
            + "\n gameVer= " + request.gameVer + "\n osScnCd= " + request.osScnCd + "\n langCd= " + request.langCd
            + "\n timeZone= " + request.timeZone + "\n bgnLvl= " + request.bgnLvl
        );

        yield return StartCoroutine(UWR_Post<Request_DN_Setting, DN_Response>(request, "https://prd-brs-relay-model.mathpid.com/api/v1/contest/diagnosis/setting", 
            isSendAuth: false)); //* 唯一に診断評価スタートのみ 権限送り(トークン)が falseになっている

        onGetDiagnosis.Invoke();

        yield return null;
    }

    /// <summary>
    /// 진단평가 매 문제 풀이 시 서버와 통신 (정답 골랐을 때)
    /// </summary>
    private IEnumerator SendProgress_Diagnosis(string _prgsCd, string _qstCd, string _qstCransr, string _ansrCwYn, long _sid, long _nQstDelayTime)
    {
        Request_DN_Progress request = new Request_DN_Progress();
        request.gameCd = strGameCD;
        request.mbrId = strMBR_ID;
        request.prgsCd = _prgsCd;// "W";    // W: 진단 진행    E: 진단 완료    X: 기타 취소?
        request.qstCd = _qstCd;             // 문항 코드
        request.qstCransr = _qstCransr;     // 입력한 답내용
        request.ansrCwYn = _ansrCwYn;//"Y"; // 정답 여부
        request.sid = _sid;                 // 진단 ID
        request.slvTime = _nQstDelayTime;   //5000;

        Debug.Log("<color=yellow>SendProgress_Diagnosis:: Request_DN_Progress</color>"
            + "\n gameCd= " + request.gameCd
            + "\n mbrId= " + request.mbrId
            + "\n prgsCd= " + request.prgsCd
            + "\n qstCd= " + request.qstCd
            + "\n qstCransr= " + request.qstCransr
            + "\n ansrCwYn= " + request.ansrCwYn
            + "\n sid= " + request.sid
            + "\n slvTime= " + request.slvTime
        );

        yield return StartCoroutine(UWR_Post<Request_DN_Progress, DN_Response>(request, "https://prd-brs-relay-model.mathpid.com/api/v1/contest/diagnosis/progress", 
            isSendAuth: true));

        onGetDiagnosis.Invoke();

        yield return null;
    }

    /// <summary>
    /// 학습 문제를 받아오기 위해 서버와 통신
    /// </summary>
    private IEnumerator Send_Learning()
    {
        Request_Learning_Setting request = new Request_Learning_Setting();

        request.gameCd = strGameCD;
        request.mbrId = strMBR_ID;
        request.gameVer = strGameVer;
        request.osScnCd = strOsScnCd;
        request.deviceNm = strDeviceNm;
        request.langCd = "KO";
        request.timeZone = TimeZoneInfo.Local.BaseUtcOffset.Hours;

        request.mathpidId = "";

        Debug.Log("<color=red>Send_Learning:: Request_Learning_Setting</color>"
            + "\n gameCd= " + request.gameCd + "\n mbrId= " + request.mbrId + "\n gameVer= " + request.gameVer
            + "\n osScnCd= " + request.osScnCd + "\n deviceNm= " + request.deviceNm + "\n timeZone= " + request.timeZone
            + "\n mathpidId= " + request.mathpidId
        );

        yield return StartCoroutine(UWR_Post<Request_Learning_Setting, Response_Learning_Setting>(request, "https://prd-brs-relay-model.mathpid.com/api/v1/contest/learning/setting", 
            isSendAuth: true));

        onGetLearning.Invoke();

        cMyAnsrs = new List<Learning_MyAnsr>();

        yield return null;
    }

    /// <summary>
    /// 학습 8문제 완료. 서버와 통신하여 결과를 받아옴
    /// </summary>
    private IEnumerator SendProgress_Learning()
    {
        Request_Learning_Progress request = new Request_Learning_Progress();

        request.gameCd = strGameCD;
        request.mbrId = strMBR_ID;
        request.prgsCd = "E";
        request.sid = cLearnSet.data.sid;
        request.bgnDt = cLearnSet.data.bgnDt;
        request.data = cMyAnsrs;

        //* ログ
        int i = 0;
        Debug.Log("<color=yellow>SendProgress_Learning:: Request_Learning_Progress</color>"
            + "\n gameCd= " + request.gameCd + "\n mbrId= " + request.mbrId + "\n prgsCd= " + request.prgsCd
            + "\n sid= " + request.sid + "\n bgnDt= " + request.bgnDt
        );
        request.data.ForEach(dt => {
            Debug.Log($"<color=yellow>data{i++}: dt.qstCd({dt.qstCd}), dt.qstCransr({dt.qstCransr}), dt.ansrCwYn({dt.ansrCwYn}), dt.slvTime({dt.slvTime}ms)</color>");
        });

        yield return StartCoroutine(UWR_Post<Request_Learning_Progress, Response_Learning_Progress>(request, "https://prd-brs-relay-model.mathpid.com/api/v1/contest/learning/progress", 
            isSendAuth: true));

        //* 結果パンネル 表示
        Debug.Log("lrnPrgsStsCd= " + cLearnProg.data.lrnPrgsStsCd);
        StartCoroutine(GM._.rm.coDisplayResultPanel(this));

        yield return null;
    }

    /// <summary>
    /// UnityWebRequest를 사용하여 서버와 실제로 통신
    /// </summary>
    /// <typeparam name="TRequest"> 보내기 원하는 타입 </typeparam>
    /// <typeparam name="TResponse"> 받아올 타입 </typeparam>
    /// <param name="request"> 보내는 값 </param>
    /// <param name="url"> URL </param>
    /// <param name="isSendAuth"> Authorization 헤더 보낼지 </param>
    /// <returns></returns>
    private IEnumerator UWR_Post<TRequest, TResponse>(TRequest request, string url, bool isSendAuth)
    where TRequest : class
    where TResponse : class
    {
        string strBody = JsonUtility.ToJson(request);

        using (UnityWebRequest uwr = UnityWebRequest.PostWwwForm(url, string.Empty))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(strBody);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("x-api-key", strGameKey);

            if (isSendAuth) uwr.SetRequestHeader("Authorization", strAuthorization);

            uwr.timeout = 5;
            yield return uwr.SendWebRequest();

            Debug.Log($"□Request => {strBody}");

            if (uwr.error == null)  //성공 시
            {
                TResponse output = default;
                try
                {
                    output = JsonUtility.FromJson<TResponse>(uwr.downloadHandler.text);
                }
                catch (Exception e) { Debug.LogError(e.Message); }

                cDiagnotics = null;
                cLearnSet = null;
                cLearnProg = null;

                switch (output)
                {
                    case DN_Response dnResponse:
                        cDiagnotics = dnResponse;
                        qstCransr = cDiagnotics.data.qstCransr;
                        break;

                    case Response_Learning_Setting ResponselearnSet:
                        cLearnSet = ResponselearnSet;
                        break;

                    case Response_Learning_Progress ResponselearnProg:
                        cLearnProg = ResponselearnProg;
                        break;

                    default:
                        Debug.LogError("type error - output type : " + output.GetType().ToString());
                        break;
                }

                //* 認証 習得
                if (uwr.GetResponseHeaders().ContainsKey("Authorization")) {
                    strAuthorization = uwr.GetResponseHeader("Authorization");
                    DB.Dt.MyAuthorization = strAuthorization;
                }
            }
            else //실패 시
            {
                Debug.LogError("결과를 받아오는 데 실패했습니다.");
                Debug.LogError(uwr.error.ToString());
            }
            
            /*
                ? 学習結果
                ■Response => {
                    "result":true, 
                    "msg":"success",
                    "data":{
                        "explSpedCd":"ESC02",    //? 解く速度コード：ESC01(遅い)、ESC02(普通)、ESC03(早い)
                        "explSped":80,           //? 解く速度値：0 ~ 100
                        "lrnPrgsStsCd":"LPSC02", //? 結果進行コード：LPS01(動力)、LPS02(基本)、LPS03(充分)、LPS04(完璧)
                        "acrcyCd":"A",           //? 解く正確度コード：D(未達)、C(普通)、B(高い)、A(完璧)
                        "explAcrcyRt":100        //? 解く正確度値：0 ~ 100
                    }
                }
            */
            
            Debug.Log($"■Response => {uwr.downloadHandler.text}");
            uwr.Dispose();
        }
    }
    #endregion

    #region PUBLIC METHOD
    public void FirstRun_Diagnosis(int diffLevel) {
        StartCoroutine(Send_Diagnosis(diffLevel));
    }

    public void Learning_GetQuestion() {
        StartCoroutine(Send_Learning());
    }

    /// <summary>
    /// 진단 - 고른 정답을 저장 후 각 문제마다 전송 
    /// </summary>
    public void Diagnosis_SelectAnswer(string _cransr, string _ansrYn, long _slvTime = 5000)
    {
        long sid            = cDiagnotics.data.sid;
        string prgsCd       = cDiagnotics.data.prgsCd;
        string qstCd        = cDiagnotics.data.qstCd;

        string qstCransr    = _cransr;
        string ansrCwYn     = _ansrYn;
        long slvTime        = _slvTime;

        StartCoroutine(SendProgress_Diagnosis(prgsCd, qstCd, qstCransr, ansrCwYn, sid, slvTime));
    }

    /// <summary>
    /// 학습 - 고른 정답을 저장 후 푼 문제가 8개가 되면 딱 한번 전송
    /// </summary>
    public void Learning_SelectAnswer(int _index, string _cransr, string _ansrYn, long _slvTime = 5000)
    {
        if(cMyAnsrs == null) cMyAnsrs = new List<Learning_MyAnsr>();

        cMyAnsrs.Add(new Learning_MyAnsr(cLearnSet.data.qsts[_index - 1].qstCd, _cransr, _ansrYn, _slvTime));

        if(cMyAnsrs.Count >= 8) {
            StartCoroutine(SendProgress_Learning());
        }
    }
    #endregion

    #region ForTest
    public void Diagnosis_SelectAnswer_Forced()
    {
        long sid = cDiagnotics.data.sid;
        string prgsCd = cDiagnotics.data.prgsCd;
        string qstCd = cDiagnotics.data.qstCd;

        string qstCransr = cDiagnotics.data.qstCransr;
        string ansrCwYn = "Y";
        long slvTime = 5000;

        StartCoroutine(SendProgress_Diagnosis(prgsCd, qstCd, qstCransr, ansrCwYn, sid, slvTime));
    }
    #endregion


}
