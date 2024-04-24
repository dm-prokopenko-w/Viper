namespace Game
{
    public static class Constants
    {
        public const string PlayerConfigPath = "PlayerConfig";
        public const string CellsConfigPath = "CellsConfig";
        public const string EnemyConfigPath = "EnemyConfig";
        public const string BonusConfigPath = "BonusConfig";

        public const string RectTransformViewID = "RectTransformViewID";
        public const string GridLayoutViewID = "GridLayoutViewID";
        
        public const string ActivePopupID = "ActivePopup";
        
        public const string AnimatorViewID = "CameraAnimator";
        public const string TextViewID = "TextViewID";
        public const string ButtonViewID = "ButtonViewID";
        public const string TransformViewID = "TransformViewID";

        public const string StartKey = "Start";
        public const string ShowKey = "Show";
        public const string HideKey = "Hide";
        
        public const string TimeStepCountText = "Step for sec: ";
        public const string BonusCountText = "Bonus: ";
        
        public enum PopupsID
        {
            None,
            Win,
            Lose,
            Start,
        }

        public enum TransformObject
        {
            None,
        }

        public enum RectTransformObject
        {
            None,
            CellsParent,
            
            ActivePlayerBodyParent,
            InactivePlayerBodyParent,
            
            HeadParent,
            InactiveEnemyParent,
        }

        public enum ButtonObject
        {
            None,
            StartGame,
            RotateLeft,
            RotateRight,
        }
        
        public enum TextObject
        {
            None,
            BodyCount,
            TimeStep,
        }
        
        public enum AnimatorObject
        {
            None,
        }
        
        public enum VFXObjectType
        {
            None,
        }
        
        public enum CellType
        {
            None,
            Head,
            Body,
            Enemy,
            Bonus,
        }
        
        public enum Dir
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}