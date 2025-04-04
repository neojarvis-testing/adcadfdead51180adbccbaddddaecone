using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace dotnetapp.Tests
{
    [TestFixture]
    public class ReflectionTests
    {
        private Assembly assembly;
        private Type mobileType;
        private Type mobileManagerType;

        [SetUp]
        public void Setup()
        {
            assembly = Assembly.LoadFrom("dotnetapp.dll");
            mobileType = assembly.GetType("Mobile");
            mobileManagerType = assembly.GetType("MobileManager");
        }

        [Test]
        public void Test_MobileClass_ShouldExist()
        {
            Assert.IsNotNull(mobileType, "Mobile class should exist.");
        }

        [Test]
        public void Test_MobileManagerClass_ShouldExist()
        {
            Assert.IsNotNull(mobileManagerType, "MobileManager class should exist.");
        }

        [Test]
        public void Test_MobileClass_ShouldHaveCorrectProperties()
        {
            Assert.IsNotNull(mobileType.GetProperty("MobileId"), "Mobile class should have a 'MobileId' property.");
            Assert.IsNotNull(mobileType.GetProperty("Brand"), "Mobile class should have a 'Brand' property.");
            Assert.IsNotNull(mobileType.GetProperty("Model"), "Mobile class should have a 'Model' property.");
            Assert.IsNotNull(mobileType.GetProperty("Price"), "Mobile class should have a 'Price' property.");
            Assert.IsNotNull(mobileType.GetProperty("LaunchedYear"), "Mobile class should have a 'LaunchedYear' property.");
        }

        [Test]
        public void Test_MobileManagerClassMethods_ShouldExist()
        {
            Assert.IsNotNull(mobileManagerType.GetMethod("AddMobile"), "MobileManager class should have an 'AddMobile' method.");
            Assert.IsNotNull(mobileManagerType.GetMethod("DisplayMobiles"), "MobileManager class should have a 'DisplayMobiles' method.");
            Assert.IsNotNull(mobileManagerType.GetMethod("SearchMobileByBrand"), "MobileManager class should have a 'SearchMobileByBrand' method.");
            Assert.IsNotNull(mobileManagerType.GetMethod("DeleteMobile"), "MobileManager class should have a 'DeleteMobile' method.");
        }

        [Test]
        public void Test_AddMobile_ShouldAddMobileAndDisplayDetails()
        {
            var mobile = Activator.CreateInstance(mobileType);
            mobileType.GetProperty("MobileId").SetValue(mobile, 201);
            mobileType.GetProperty("Brand").SetValue(mobile, "Samsung");
            mobileType.GetProperty("Model").SetValue(mobile, "Galaxy S21");
            mobileType.GetProperty("Price").SetValue(mobile, 799.99m);
            mobileType.GetProperty("LaunchedYear").SetValue(mobile, 2021);

            var method = mobileManagerType.GetMethod("AddMobile");
            var instance = Activator.CreateInstance(mobileManagerType);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                method.Invoke(instance, new object[] { mobile });
                var output = sw.ToString().Trim();

                // Case-insensitive check
                StringAssert.Contains("Mobile with ID 201 added successfully".ToLower(), output.ToLower());
            }
        }

        [Test]
        public void Test_AddMobile_ShouldNotAddDuplicateMobile()
        {
            var mobile = Activator.CreateInstance(mobileType);
            mobileType.GetProperty("MobileId").SetValue(mobile, 101);
            mobileType.GetProperty("Brand").SetValue(mobile, "Samsung");
            mobileType.GetProperty("Model").SetValue(mobile, "Galaxy S21");
            mobileType.GetProperty("Price").SetValue(mobile, 799.99m);
            mobileType.GetProperty("LaunchedYear").SetValue(mobile, 2021);

            var method = mobileManagerType.GetMethod("AddMobile");
            var instance = Activator.CreateInstance(mobileManagerType);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                method.Invoke(instance, new object[] { mobile });
                method.Invoke(instance, new object[] { mobile });
                var output = sw.ToString().Trim();

                // Case-insensitive check
                StringAssert.Contains("A mobile with ID 101 already exists".ToLower(), output.ToLower());
            }
        }

        [Test]
        public void Test_DeleteMobile_ShouldDeleteExistingMobile()
        {
            var mobile = Activator.CreateInstance(mobileType);
            mobileType.GetProperty("MobileId").SetValue(mobile, 301);
            mobileType.GetProperty("Brand").SetValue(mobile, "Samsung");
            mobileType.GetProperty("Model").SetValue(mobile, "Galaxy S21");
            mobileType.GetProperty("Price").SetValue(mobile, 799.99m);
            mobileType.GetProperty("LaunchedYear").SetValue(mobile, 2021);

            var addMethod = mobileManagerType.GetMethod("AddMobile");
            var deleteMethod = mobileManagerType.GetMethod("DeleteMobile");
            var instance = Activator.CreateInstance(mobileManagerType);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                addMethod.Invoke(instance, new object[] { mobile });
                deleteMethod.Invoke(instance, new object[] { 301 });
                var output = sw.ToString().Trim();

                // Case-insensitive check
                StringAssert.Contains("Mobile with ID 301 deleted".ToLower(), output.ToLower());
            }
        }

        [Test]
        public void Test_DeleteMobile_ShouldDisplayMobileNotFoundMessage()
        {
            var deleteMethod = mobileManagerType.GetMethod("DeleteMobile");
            var instance = Activator.CreateInstance(mobileManagerType);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                deleteMethod.Invoke(instance, new object[] { 999 });
                var output = sw.ToString().Trim();

                // Case-insensitive check
                StringAssert.Contains("Mobile with ID 999 not found".ToLower(), output.ToLower());
            }
        }

        [Test]
        public void Test_SearchMobileByBrand_ShouldDisplayMatchingMobiles()
        {
            var mobile = Activator.CreateInstance(mobileType);
            mobileType.GetProperty("MobileId").SetValue(mobile, 101);
            mobileType.GetProperty("Brand").SetValue(mobile, "Samsung");
            mobileType.GetProperty("Model").SetValue(mobile, "Galaxy S21");
            mobileType.GetProperty("Price").SetValue(mobile, 799.99m);
            mobileType.GetProperty("LaunchedYear").SetValue(mobile, 2021);

            var addMethod = mobileManagerType.GetMethod("AddMobile");
            var searchMethod = mobileManagerType.GetMethod("SearchMobileByBrand");
            var instance = Activator.CreateInstance(mobileManagerType);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                addMethod.Invoke(instance, new object[] { mobile });
                searchMethod.Invoke(instance, new object[] { "Samsung" });
                var output = sw.ToString().Trim();

                // Case-insensitive check
                StringAssert.Contains("Mobiles found for brand: Samsung".ToLower(), output.ToLower());
                StringAssert.Contains("Mobile ID: 101".ToLower(), output.ToLower());
                StringAssert.Contains("Model: Galaxy S21".ToLower(), output.ToLower());
                StringAssert.Contains("Price: 799.99".ToLower(), output.ToLower());
                StringAssert.Contains("Launched Year: 2021".ToLower(), output.ToLower());
            }
        }

        [Test]
        public void Test_SearchMobileByBrand_ShouldDisplayNoMobilesMessageForNonExistentBrand()
        {
            var searchMethod = mobileManagerType.GetMethod("SearchMobileByBrand");
            var instance = Activator.CreateInstance(mobileManagerType);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                searchMethod.Invoke(instance, new object[] { "Apple" });
                var output = sw.ToString().Trim();

                // Case-insensitive check
                StringAssert.Contains("No mobiles found for brand: Apple".ToLower(), output.ToLower());
            }
        }

        [Test]
        public void Test_DisplayMobiles_ShouldDisplayAllMobiles()
        {
            var mobile1 = Activator.CreateInstance(mobileType);
            var mobile2 = Activator.CreateInstance(mobileType);

            mobileType.GetProperty("MobileId").SetValue(mobile1, 101);
            mobileType.GetProperty("Brand").SetValue(mobile1, "Samsung");
            mobileType.GetProperty("Model").SetValue(mobile1, "Galaxy S21");
            mobileType.GetProperty("Price").SetValue(mobile1, 799.99m);
            mobileType.GetProperty("LaunchedYear").SetValue(mobile1, 2021);

            mobileType.GetProperty("MobileId").SetValue(mobile2, 102);
            mobileType.GetProperty("Brand").SetValue(mobile2, "Apple");
            mobileType.GetProperty("Model").SetValue(mobile2, "iPhone 13");
            mobileType.GetProperty("Price").SetValue(mobile2, 999.99m);
            mobileType.GetProperty("LaunchedYear").SetValue(mobile2, 2021);

            var addMethod = mobileManagerType.GetMethod("AddMobile");
            var displayMethod = mobileManagerType.GetMethod("DisplayMobiles");
            var instance = Activator.CreateInstance(mobileManagerType);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                addMethod.Invoke(instance, new object[] { mobile1 });
                addMethod.Invoke(instance, new object[] { mobile2 });
                displayMethod.Invoke(instance, null);
                var output = sw.ToString().Trim();

                // Case-insensitive check
                StringAssert.Contains("Mobile ID: 101".ToLower(), output.ToLower());
                StringAssert.Contains("Brand: Samsung".ToLower(), output.ToLower());
                StringAssert.Contains("Model: Galaxy S21".ToLower(), output.ToLower());
                StringAssert.Contains("Mobile ID: 102".ToLower(), output.ToLower());
                StringAssert.Contains("Brand: Apple".ToLower(), output.ToLower());
                StringAssert.Contains("Model: iPhone 13".ToLower(), output.ToLower());
            }
        }

        [Test]
        public void Test_DisplayMobiles_ShouldDisplayNoMobilesAvailableMessage()
        {
            var displayMethod = mobileManagerType.GetMethod("DisplayMobiles");
            var instance = Activator.CreateInstance(mobileManagerType);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                displayMethod.Invoke(instance, null);
                var output = sw.ToString().Trim();

                // Case-insensitive check
                StringAssert.Contains("No mobiles available".ToLower(), output.ToLower());
            }
        }
    }
}
