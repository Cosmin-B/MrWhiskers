﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class ResolutionProfilerView : MyBehaviour, IEventReceiver<OnResolutionScreen>
{
    [SerializeField] List<TMP_Text> profileTexts = new List<TMP_Text>();
    [SerializeField] List<Image> profileImages = new List<Image>();
    [SerializeField] float animationDuration = 1f;
    [SerializeField] float animationInitialDelay = 1f;

    // Use this for initialization
    private void Start()
    {
        Assert.IsTrue(profileTexts.Count == 4);
        Assert.IsTrue(profileImages.Count == 4);
    }

    public void On(OnResolutionScreen resolution)
    {
        Dictionary<PlayerProfile, int> profiles = resolution.profiles;

        float totalAmount = profiles.Sum(p => p.Value);

        var seq = DOTween.Sequence();
        seq.AppendInterval(animationInitialDelay);

        // Set fill Amounts for images and text for percentages
        for (int i = 0; i < 4; i++)
        {
            float profileFraction = Mathf.InverseLerp(0f, totalAmount, profiles[(PlayerProfile)i]);
            
            profileTexts[i].text = Math.Round(profileFraction * 100f, 1) + "%";

            var image = profileImages[i];
            image.fillAmount = 0f;
            var tween = image
                .DOFillAmount(Mathf.Clamp(profileFraction, 0.01f, 1f), animationDuration)
                .SetEase(Ease.InQuad);
            seq.Join(tween);
        }
    }
}
