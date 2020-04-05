using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WpfProject;


public class TestAppViewModel
    {
        [Test]
        [TestCase("Bulbasaur")]
        [TestCase("charizard")]
        [TestCase("Empoleon")]
        public async Task TestCanGetPokemonByName(string expectedName )
        {
            string baseurl = "https://pokeapi.co/api/v2/pokemon/";
            ApplicationViewModel avmtest = new ApplicationViewModel();
            await avmtest.GetPokemon( (baseurl+ (expectedName.ToLower()).Trim() ));
            Console.WriteLine(avmtest.ChosenPokemon.Name);
            Assert.That(avmtest.ChosenPokemon.Name.ToLower(), Is.EqualTo( (expectedName.ToLower()).Trim() ) );
        }
        
        [Test]
        [TestCase("45")]
        [TestCase("9")]
        [TestCase("1 ")]
        public async Task TestCanGetPokemonByID(string expectedID)
        {
            string baseurl = "https://pokeapi.co/api/v2/pokemon/";
            ApplicationViewModel avmtest = new ApplicationViewModel();
            await avmtest.GetPokemon( (baseurl+ (expectedID) ));
            Console.WriteLine(avmtest.ChosenPokemon.Name);
            Assert.That(avmtest.ChosenPokemon.Name.ToLower(), Is.EqualTo( (expectedID.ToLower()).Trim() ) );
        }

        [Test]
        public void TestCanGetPokesearchByDefault()
        {
            ApplicationViewModel avmtest = new ApplicationViewModel();
            Assert.That(avmtest.Pokesearch, Is.EqualTo("Bulbasaur"));
        }
        
    }
