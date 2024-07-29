using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.TestTools;

public class StaminaControllerTest
{
    private GameObject testObject;
    private StaminaController staminaController;
    private bool eventInvoked;

    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and add the StaminaController component
        testObject = new GameObject("TestObject");
        staminaController = testObject.AddComponent<StaminaController>();

        // Set default stamina values
        staminaController._maximumStamina = 100f;
        staminaController._currentStamina = 50f;

        // Setup event listener
        eventInvoked = false;
        staminaController.OnStaminaChanged = new UnityEvent();
        staminaController.OnStaminaChanged.AddListener(() => eventInvoked = true);
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the test object after each test
        Object.DestroyImmediate(testObject);
    }

    [Test]
    public void ConsumeStamina_ReducesStaminaCorrectly()
    {
        // Act
        staminaController.ConsumeStamina(20f);

        // Assert
        Assert.AreEqual(30f, staminaController._currentStamina);
        Assert.IsTrue(eventInvoked, "OnStaminaChanged event was not invoked");
    }

    [Test]
    public void ConsumeStamina_DoesNotGoBelowZero()
    {
        // Act
        staminaController.ConsumeStamina(60f);

        // Assert
        Assert.AreEqual(50f, staminaController._currentStamina);
        Assert.IsFalse(eventInvoked, "OnStaminaChanged event should not be invoked when no stamina is consumed");
    }

    [Test]
    public void RecoverStamina_IncreasesStaminaCorrectly()
    {
        // Act
        staminaController.RecoverStamina(30f);

        // Assert
        Assert.AreEqual(80f, staminaController._currentStamina);
        Assert.IsTrue(eventInvoked, "OnStaminaChanged event was not invoked");
    }

    [Test]
    public void RecoverStamina_DoesNotExceedMaximum()
    {
        // Act
        staminaController.RecoverStamina(60f);

        // Assert
        Assert.AreEqual(100f, staminaController._currentStamina);
        Assert.IsTrue(eventInvoked, "OnStaminaChanged event was not invoked");
    }

    [Test]
    public void AddMaxStamina_IncreasesMaximumAndCurrentStamina()
    {
        // Act
        staminaController.AddMaxStamina(20f);

        // Assert
        Assert.AreEqual(120f, staminaController._maximumStamina);
        Assert.AreEqual(70f, staminaController._currentStamina);
        Assert.IsTrue(eventInvoked, "OnStaminaChanged event was not invoked");
    }

    [Test]
    public void UpdateMaxStamina_ChangesMaximumStamina()
    {
        // Act
        staminaController.UpdateMaxStamina(150f);

        // Assert
        Assert.AreEqual(150f, staminaController._maximumStamina);
        Assert.IsTrue(eventInvoked, "OnStaminaChanged event was not invoked");
    }

    [Test]
    public void UpdateCurrStamina_ChangesCurrentStamina()
    {
        // Act
        staminaController.UpdateCurrStamina(40f);

        // Assert
        Assert.AreEqual(40f, staminaController._currentStamina);
        Assert.IsTrue(eventInvoked, "OnStaminaChanged event was not invoked");
    }

    [Test]
    public void ResetStamina_ResetsStaminaToMaximum()
    {
        // Act
        staminaController.ResetStamina();

        // Assert
        Assert.AreEqual(100f, staminaController._currentStamina);
        Assert.IsTrue(eventInvoked, "OnStaminaChanged event was not invoked");
    }

    [Test]
    public void RemainingStaminaPercentage_CalculatesCorrectly()
    {
        // Assert
        Assert.AreEqual(0.5f, staminaController.RemainingStaminaPercentage);
    }
    
    [Test]
    public void RemainingStaminaPercentage_RoundOffsCorrectly()
    {
        staminaController._maximumStamina = 13.7f;
        staminaController._currentStamina = 3.8f;
        Assert.AreEqual(0.277f, staminaController.RemainingStaminaPercentage, 0.001f);
    }

   
}
