namespace Game
{
    public static class Constants
    {
        public const string CellsConfigPath = "CellsConfig";

        public const string RectTransformViewID = "RectTransformViewID";
        public const string GridLayoutViewID = "GridLayoutViewID";
        
        public const string PlayerConfigPath = "PlayerConfig";

        public const string ActivePopupID = "ActivePopup";
        
        public const string AnimatorViewID = "CameraAnimator";
        public const string TextViewID = "TextViewID";
        public const string ButtonViewID = "ButtonViewID";
        public const string TransformViewID = "TransformViewID";

        public const string ShowKey = "Show";
        public const string HideKey = "Hide";
        
        public const string StepCountText = "Step: ";
        public const string FruitsCountText = "Fruits: ";
        
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
            Block,
            Fruit,
        }
    }
}