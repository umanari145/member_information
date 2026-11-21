using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2.modCommon
{
    public static class Messages
    {

        public static class SearchForm
        {
            public const string EditOpenError = "編集画面を開けませんでした。";
        }

        public static class RegistForm
        {
            public const string LoadFailed = "会員データの読み込みに失敗しました。";

            public const string InsertSuccess = "新規会員を登録しました。";
            public const string UpdateSuccess = "会員情報を更新しました。";
            public const string RegistFailed = "会員データの登録に失敗しました。";
        }

        public static class DBError
        {
            public const string CannotOpenFile = "接続文字列 {0} が appsettings.json に見つかりません。";
            public const string NotExistInsertColumn = "INSERT 対象の列がありません。";

            public const string NotExistUpdateColumn = "UPDATE 対象の列がありません。";
        }

        public static class ImportFileError
        {
            public const string CannotOpneFile = "ファイルの開閉に失敗しました。";

            public const string EmptyLine = "内容が入力されていません。";
            public const string ShortageItem = "項目数が不足しています。";
            public const string ExceptionItem = "例外が発生しました。";
        }

        public static class ListMember
        {
            public const string SelectError = "会員データの取得に失敗しました。";
            public const string NoDeleteData = "削除する項目が選択されていません。";
            public const string DeleteConfirm = "選択された {0} 件のデータを削除しますか？";
            public const string DeleteSuccess = "削除が完了しました。";
            public const string DeleteError = "削除に失敗しました。";
        }
    }
}
