using System;
using System.Numerics;
using System.ComponentModel;
using System.Diagnostics;
using DGD203_2;

public class Map
{
	private Game _theGame;

	private Vector2 _coordinates;

	private int[] _widthBoundaries;
	private int[] _heightBoundaries;

	private Location[] _locations;


	public Map(Game game, int width, int height)
	{
		_theGame = game;

		// Setting the width boundaries
		int widthBoundary = (width - 1) / 2;

        _widthBoundaries = new int[2];
        _widthBoundaries[0] = -widthBoundary;
		_widthBoundaries[1] = widthBoundary;

		// Setting the height boundaries
        int heightBoundary = (height - 1) / 2;

        _heightBoundaries = new int[2];
		_heightBoundaries[0] = -heightBoundary;
		_heightBoundaries[1] = heightBoundary;

		// Setting starting coordinates
        _coordinates = new Vector2(0,0);

		GenerateLocations();

		// Display result message
		Console.WriteLine($"Created map with size {width}x{height}");
    }

    #region Coordinates

    public Vector2 GetCoordinates()
	{
		return _coordinates;
	}

	public void SetCoordinates(Vector2 newCoordinates)
	{
		_coordinates = newCoordinates;
	}

	#endregion

	#region Movement

	public void MovePlayer(int x, int y)
	{
		int newXCoordinate = (int)_coordinates[0] + x;
        int newYCoordinate = (int)_coordinates[1] + y;

		if (!CanMoveTo(newXCoordinate, newYCoordinate)) 
		{
            Console.WriteLine("You can't go that way");
            return;
        }

		_coordinates[0] = newXCoordinate;
		_coordinates[1] = newYCoordinate;

		CheckForLocation(_coordinates);
    }

	private bool CanMoveTo(int x, int y)
	{
		return !(x < _widthBoundaries[0] || x > _widthBoundaries[1] || y < _heightBoundaries[0] || y > _heightBoundaries[1]);
	}

	#endregion

	#region Locations

	private void GenerateLocations()
	{
        _locations = new Location[6];

        Vector2 houseLocation = new Vector2(0, 0);
        List<Item> houseItems = new List<Item>();
        houseItems.Add(Item.Serum);
        Location house = new Location("House", LocationType.City, houseLocation, houseItems);
        _locations[0] = house;


        Vector2 prisonLocation = new Vector2(-2, 2);
        List<Item> prisonItems = new List<Item>();
        prisonItems.Add(Item.Bluepill);
        Location prison = new Location("Prison", LocationType.City, prisonLocation, prisonItems);
        _locations[1] = prison;


        Vector2 hospitalLocation = new Vector2(1, -2);
        List<Item> hospitalItems = new List<Item>();
        hospitalItems.Add(Item.Redpill);
        Location hospital = new Location("Hospital", LocationType.City, hospitalLocation, hospitalItems);
        _locations[2] = hospital;


        Vector2 botanicaLocation = new Vector2(1, 1);
        Location botanica = new Location("Botanica", LocationType.City, botanicaLocation);
        _locations[3] = botanica;


        Vector2 firstCombatLocation = new Vector2(-2, 1);
		Location firstCombat = new Location("First Combat", LocationType.Combat, firstCombatLocation);
		_locations[4] = firstCombat;

        Vector2 secondCombatLocation = new Vector2(-1, -1);
        Location secondCombat = new Location("Second Combat", LocationType.Combat, secondCombatLocation);
        _locations[5] = secondCombat;

    }

	public void CheckForLocation(Vector2 coordinates)
	{
        Console.WriteLine($"You are now standing on {_coordinates[0]},{_coordinates[1]}");

        if (IsOnLocation(_coordinates, out Location location))
        {
            if (location.Type == LocationType.Combat)
			{
				if (location.CombatAlreadyHappened) return;

				Console.WriteLine("Get ready for war soldier!");
				Combat combat = new Combat(_theGame, location);

				combat.StartCombat();

			} else
			{
				Console.WriteLine($"You are in {location.Name} {location.Type}");

				if (HasItem(location))
				{
					Console.WriteLine($"There is a {location.ItemsOnLocation[0]} here");
				}
			}
        }
    }

	private bool IsOnLocation(Vector2 coords, out Location foundLocation)
	{
		for (int i = 0; i < _locations.Length; i++)
		{
			if (_locations[i].Coordinates == coords)
			{
				foundLocation = _locations[i];
				return true;
			}
		}

		foundLocation = null;
		return false;
	}

	private bool HasItem(Location location)
	{
		return location.ItemsOnLocation.Count != 0;

		// ---- THE LONG FORM ----
		//if (location.ItemsOnLocation.Count == 0)
		//{
		//	return false;
		//} else
		//{
		//	return true;
		//}
	}

	public void TakeItem(Location location)
	{

	}

	public void TakeItem(Player player, Vector2 coordinates)
	{
		if (IsOnLocation(coordinates, out Location location))
		{
			if (HasItem(location))
			{
				Item itemOnLocation = location.ItemsOnLocation[0];

				player.TakeItem(itemOnLocation);
				location.RemoveItem(itemOnLocation);

				Console.WriteLine($"You took the {itemOnLocation} I don't know what it does right now, but I guess it will be useful in the future.");

				return;
			}
		}

		Console.WriteLine("Are you dreaming? Soldier, there is nothing to take here!");
	}

	public void RemoveItemFromLocation(Item item)
	{
		for (int i = 0; i < _locations.Length; i++)
		{
			if (_locations[i].ItemsOnLocation.Contains(item))
			{
				_locations[i].RemoveItem(item);
			}
		}
	}

	#endregion
}