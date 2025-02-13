﻿using System;
using LitMotion;

public partial class AnimationConfig
{
    [Serializable]
    public class ItemConfig
    {
        public float itemTransferDuration = .2f;
        public float itemTransferStartDelay = .1f;
        public Ease itemTransferEase;
    }
    [Serializable]
    public class TrayConfig
    {
        public float releaseTrayDuration = .1f;
        public Ease releaseTrayEase;
    
        public float destroyTrayDuration = .1f;
        public Ease destroyTrayEase;
    
        public float clearTrayDelay = 0.2f;
    }
    [Serializable]
    public class SlotConfig
    {
        public float normalSlotDropTime = 0.5f;
        public Ease dropDownEaseZigzag = Ease.OutBounce;
        public Ease dropDownEaseWave = Ease.OutQuart;
    }
    [Serializable]
    public class GridConfig
    {
        public float rowDelayFactor = 0.1f; // Thời gian delay giữa các hàng
        public float columnDelayFactor = 0.05f; // Thời gian delay giữa các cột
    }

    [Serializable]
    public class LevelUIConfig
    {
        public float rotateSpeed = 5;
        public float lerpingValue = 5;
    }
    [Serializable]
    public class TopUIConfig
    {
        // Heart
        public Ease heartDropEase = Ease.OutBack;
        public float heartDropTime = 0.5f;
        public float heartDropDelay = 0.1f;
        // avatar
        public float avatarScaleTime = 0.35f;
        public Ease scaleUpEase = Ease.OutQuad;
        public Ease scaleDownEase = Ease.InQuart;

    }
}