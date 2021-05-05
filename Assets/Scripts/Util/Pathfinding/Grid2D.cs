using System;
using System.Collections.Generic;
using UnityEngine;
using Util.Pathfinding.SearchAlgorithms;

namespace Util.Pathfinding
{
    /// <summary>
    /// A fixed-size two-dimensional grid class.
    /// </summary>
    /// <typeparam name="T">The grid element type.</typeparam>
    [Serializable]
    public class Grid2D<T>
    {
        /// <summary>
        /// Initializes a new instance of Grid2D with the given dimensions.
        /// </summary>
        public Grid2D(int width, int height)
        {
            if (width < 0) throw new ArgumentOutOfRangeException("Width must be greater than zero.");
            if (height < 0) throw new ArgumentOutOfRangeException("Height must be greater than zero.");

            _width = width;
            Values = new T[width * height];
        }

        /// <summary>
        /// Initializes a new instance of Grid2D with the given size.
        /// </summary>
        public Grid2D(Vec2 size) : this(size.x, size.y)
        {
        }

        /// <summary>
        /// Gets the size of the grid.
        /// </summary>
        public Vec2 Size => new Vec2(Width, Height);

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int Width => _width;

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int Height => Values.Length / _width;

        /// <summary>
        /// Gets and sets the array element at the given position.
        /// </summary>
        /// <param name="position">The position of the element. Must be within bounds.</param>
        /// <exception cref="IndexOutOfBoundsException">if the position is out of bounds.</exception>
        public T this[Vec2 position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }

        public T this[int x, int y]
        {
            get => Values[GetIndex(x, y)];
            set => Values[GetIndex(x, y)] = value;
        }

        /// <summary>
        /// Fills all of the elements in the array with values returned by the given callback
        /// </summary>
        /// <param name="callback">The function to call for each element in the array</param>
        public void Fill(Func<Vec2, T> callback)
        {
            foreach (Vec2 position in new Rect(Size))
            {
                this[position] = callback(position);
            }
        }

        protected int GetIndex(int x, int y)
        {
            return (y * _width) + x;
        }

        /*#region IPathfindingGraph Members
        
        public IPathfindingNode GetNodeAt(Vec2 coordinates)
        {
            return this[coordinates.x, coordinates.y];
        }

        public IList<IPathfindingNode> FloodFill<TSearchAlgorithm>(IPathfindingNode start, IPathfindingNode goal, 
            Motility agentMotility) where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            var searchAlgorithm = new TSearchAlgorithm();
            InitializeCosts(start);
            return searchAlgorithm.Search(start, goal, agentMotility);
        }

        public IList<IPathfindingNode> FloodFill<TSearchAlgorithm>(IPathfindingNode start, IPathfindingNode goal, 
            Motility agentMotility, Func<IPathfindingNode, IPathfindingNode, float> heuristic) 
            where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            var searchAlgorithm = new TSearchAlgorithm();
            InitializeCosts(start);
            return searchAlgorithm.Search(start, goal, agentMotility, heuristic);
        }

        public IDictionary<IPathfindingNode, IPathfindingNode> FloodFill<TFloodFillAlgorithm>(IPathfindingNode start, 
            float searchDepth, Motility agentMotility) where TFloodFillAlgorithm : IFloodFillAlgorithm, new()
        {
            var fillAlgorithm = new TFloodFillAlgorithm();
            InitializeCosts(start);
            return fillAlgorithm.Fill(start, searchDepth, agentMotility);
        }

        private void InitializeCosts(IPathfindingNode start)
        {
            foreach (T node in _values)
            {
                node.PathCost = Mathf.Infinity;
            }

            start.PathCost = 0;
        }

        #endregion*/

        private readonly int _width;
        protected readonly T[] Values;
    }

    /// <summary>
    /// A fixed-size two-dimensional grid class that can be used as a pathfinding graph.
    /// </summary>
    /// <typeparam name="T">The grid element type.</typeparam>
    /*[Serializable]
    public class Graph2D<T> : Grid2D<T>, IPathfindingGraph<T> where T : class, IPathfindingNode<T>
    {
        /// <summary>
        /// Initializes a new instance of Graph2D with the given dimensions.
        /// </summary>
        public Graph2D(int width, int height) : base(width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of Graph2D with the given size.
        /// </summary>
        public Graph2D(Vec2 size) : this(size.x, size.y)
        {
        }

        #region IPathfindingGraph Members

        public IPathfindingNode GetNodeAt(Vec2 coordinates)
        {
            return this[coordinates.x, coordinates.y];
        }

        public IList<IPathfindingNode> Pathfinder<TSearchAlgorithm>(IPathfindingNode start, IPathfindingNode goal,
            Motility agentMotility) where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            var searchAlgorithm = new TSearchAlgorithm();
            InitializeCosts(start);
            return searchAlgorithm.Search(start, goal, agentMotility);
        }

        public IList<IPathfindingNode> Pathfinder<TSearchAlgorithm>(IPathfindingNode start, IPathfindingNode goal,
            Motility agentMotility, Func<IPathfindingNode, IPathfindingNode, float> heuristic)
            where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            var searchAlgorithm = new TSearchAlgorithm();
            InitializeCosts(start);
            return searchAlgorithm.Search(start, goal, agentMotility, heuristic);
        }

        public IDictionary<IPathfindingNode, IPathfindingNode> Pathfinder<TFloodFillAlgorithm>(IPathfindingNode start,
            float searchDepth, Motility agentMotility) where TFloodFillAlgorithm : IFloodFillAlgorithm, new()
        {
            var fillAlgorithm = new TFloodFillAlgorithm();
            InitializeCosts(start);
            return fillAlgorithm.Fill(start, searchDepth, agentMotility);
        }

        private void InitializeCosts(IPathfindingNode start)
        {
            foreach (T node in Values)
            {
                node.PathCost = Mathf.Infinity;
            }

            start.PathCost = 0;
        }

        #endregion
    }*/
}