//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace BlueSheep.Common.Data.Pathfinding
//{
//    class DStar
//    {
//    }

//    struct coords
//    {
//        int x, y;
//    };

//    struct cellInfo
//    {
//        double g;
//        double rhs;
//        double cost;

//    };


//}

//using System.Collections.Generic;
//using System;

//public class state
//{
//  public int x;
//  public int y;
//}

//public class ipoint2
//{
//  public int x;
//  public int y;
//}

//public class cellInfo
//{

//  public double g;
//  public double rhs;
//  public double cost;

//}

//public class state_hash
//{
////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
////ORIGINAL LINE: uint operator ()(const state &s) const
////C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
//  public static uint operator (state s)
//  {
//    return s.x + 34245 * s.y;
//  }
//}




//public class Dstar
//{



//  /* void Dstar::Dstar()
//   * --------------------------
//   * Constructor sets constants.
//   */
//  public Dstar()
//  {

//    maxSteps = 80000; // node expansions before we give up
//    C1 = 1; // cost of an unseen cell

//  }

//  /* void Dstar::init(int sX, int sY, int gX, int gY)
//   * --------------------------
//   * Init dstar with start and goal coordinates, rest is as per
//   * [S. Koenig, 2002]
//   */
//  public void init(int sX, int sY, int gX, int gY)
//  {

//    cellHash.clear();
//    path.Clear();
//    openHash.clear();
//    while (!openList.empty())
//    {
//        openList.pop();
//    }

//    k_m = 0;

//    s_start.x = sX;
//    s_start.y = sY;
//    s_goal.x = gX;
//    s_goal.y = gY;

//    cellInfo tmp = new cellInfo();
//    tmp.g = tmp.rhs = 0;
//    tmp.cost = C1;

//    cellHash[s_goal] = tmp;

//    tmp.g = tmp.rhs = heuristic(s_start, s_goal);
//    tmp.cost = C1;
//    cellHash[s_start] = tmp;
//    s_start = calculateKey(s_start);
//    s_last = s_start;

//  }

//  public void updateCell(int x, int y, double val)
//  {

//     state u = new state();

//    u.x = x;
//    u.y = y;

//    if ((u == s_start) || (u == s_goal))
//        return;

//    makeNewCell(u);
//    cellHash[u].cost = val;

//    updateVertex(u);
//  }

//  public void updateStart(int x, int y)
//  {

//    s_start.x = x;
//    s_start.y = y;

//    k_m += heuristic(s_last, s_start);
//    s_start = calculateKey(s_start);
//    s_last = s_start;

//  }


//   /* Updates the costs for all cells and computes the shortest path to
//   * goal. Returns true if a path is found, false otherwise. The path is
//   * computed by doing a greedy search over the cost+g values in each
//   * cells. In order to get around the problem of the robot taking a
//   * path that is near a 45 degree angle to goal we break ties based on
//   *  the metric euclidean(state, goal) + euclidean(state,start). 
//   */
//  public bool replan()
//  {

//    path.Clear();

//    int res = computeShortestPath();
//    if (res < 0)
//    {
//      Console.Error.Write("NO PATH TO GOAL\n");
//      return false;
//    }
//    LinkedList<state> n = new LinkedList<state>();
//    LinkedList<state>.Enumerator i;

//    state cur =s_start;

//    while (cur != s_goal)
//    {

//      path.AddLast(cur);
//      getSucc(cur, n);

//      if (n.Count == 0)
//      {
//        Console.Error.Write("NO PATH TO GOAL\n");
//        return false;
//      }

//      double cmin = INFINITY;
//      double tmin;
//      state smin = new state();

//      for (i = n.GetEnumerator(); i.MoveNext();)
//      {

//        //if (occupied(*i)) continue;
//        double val = cost(cur, i.Current);
//        double val2 = trueDist(i.Current, s_goal) + trueDist(s_start, i.Current); // (Euclidean) cost to goal + cost to pred
//        val += getG(i.Current);

//        if (close(val, cmin))
//        {
//          if (tmin > val2)
//          {
//            tmin = val2;
//            cmin = val;
//            smin = i.Current;
//          }
//        }
//        else if (val < cmin)
//        {
//          tmin = val2;
//          cmin = val;
//          smin = i.Current;
//        }
//      }
//      n.Clear();
//      cur = smin;
//    }
//    path.AddLast(s_goal);
//    return true;
//  }


//  /* 
//   * Returns the path created by replan()
//   */
//  public LinkedList<state> getPath()
//  {
//    return path;
//  }


//  private LinkedList<state> path = new LinkedList<state>();

//  private double C1;
//  private double k_m;
//  private state s_start = new state();
//  private state s_goal = new state();
//  private state s_last = new state();
//  private int maxSteps;


//  /* 
//   * Returns true if x and y are within 10E-5, false otherwise
//   */
//  private bool close(double x, double y)
//  {
//    return (Math.Abs(x - y) < 0.00001);
//  }

//  /* 
//   * Checks if a cell is in the hash table, if not it adds it in.
//   */
//  private void makeNewCell(state u)
//  {

//    if (cellHash.find(u) != cellHash.end())
//        return;

//    cellInfo tmp = new cellInfo();
//    tmp.g = tmp.rhs = heuristic(u, s_goal);
//    tmp.cost = C1;
//    cellHash[u] = tmp;

//  }

//  /* 
//   * Returns the G value for state u.
//   */
//  private double getG(state u)
//  {

//    if (cellHash.find(u) == cellHash.end())
//    {
//      return heuristic(u, s_goal);
//    }
//    return cellHash[u].g;

//  }

//  /*
//   * Returns the rhs value for state u.
//   */
//  private double getRHS(state u)
//  {

//    if (u == s_goal)
//    {
//        return 0;
//    }

//    if (cellHash.find(u) == cellHash.end())
//    {
//      return heuristic(u, s_goal);
//    }
//    return cellHash[u].rhs;

//  }

//  /* 
//   * Sets the G value for state u
//   */
//  private void setG(state u, double g)
//  {
//    makeNewCell(u);
//    cellHash[u].g = g;
//  }

//  /* void Dstar::setRHS(state u, double rhs)
//   * --------------------------
//   * Sets the rhs value for state u
//   */
//  private double setRHS(state u, double rhs)
//  {
//    makeNewCell(u);
//    cellHash[u].rhs = rhs;
//  }

//  /* 
//   * Returns the 8-way distance between state a and state b.
//   */
//  private double eightCondist(state a, state b)
//  {
//    double temp;
//    double min = Math.Abs(a.x - b.x);
//    double max = Math.Abs(a.y - b.y);
//    if (min > max)
//    {
//      double temp = min;
//      min = max;
//      max = temp;
//    }
//    return ((M_SQRT2 - 1.0) * min + max);
//  }

//  /*
//   * As per [S. Koenig, 2002] except for 2 main modifications:
//   * 1. We stop planning after a number of steps, 'maxsteps' we do this
//   *    because this algorithm can plan forever if the start is
//   *    surrounded by obstacles. 
//   * 2. We lazily remove states from the open list so we never have to
//   *    iterate through it.
//   */
//  private int computeShortestPath()
//  {

//    LinkedList<state> s = new LinkedList<state>();
//    LinkedList<state>.Enumerator i;

//    if (openList.empty())
//    {
//        return 1;
//    }

//    int k = 0;
//    while ((!openList.empty()) && (openList.top() < (s_start = calculateKey(s_start))) || (getRHS(s_start) != getG((s_start))))
//    {

//      if (k++ > maxSteps)
//      {
//        Console.Error.Write("At maxsteps\n");
//        return -1;
//      }


//      state u = new state();
//      bool test = (getRHS(s_start) != getG(s_start));

//      // lazy remove
//      while (true)
//      {
//        if (openList.empty())
//        {
//            return 1;
//        }
//        u = openList.top();
//        openList.pop();

//        if (!isValid(u))
//            continue;
//        if (!(u < s_start) && (!test))
//        {
//            return 2;
//        }
//        break;
//      }

//      hash_map<state, float, state_hash, equal_to<state>>.iterator cur = openHash.find(u);
//      openHash.erase(cur);

//      state k_old = u;

//      if (k_old < calculateKey(u))
//      { // u is out of date
//        insert(u);
//      }
//      else if (getG(u) > getRHS(u))
//      { // needs update (got better)
//        setG(u, getRHS(u));
//        getPred(u, s);
//        for (i = s.GetEnumerator(); i.MoveNext();)
//        {
//          updateVertex(i.Current);
//        }
//      }
//      else
//      { // g <= rhs, state has got worse
//        setG(u, INFINITY);
//        getPred(u, s);
//        for (i = s.GetEnumerator(); i.MoveNext();)
//        {
//          updateVertex(i.Current);
//        }
//        updateVertex(u);
//      }
//    }
//    return 0;
//  }

//  private void updateVertex(state u)
//  {

//    LinkedList<state> s = new LinkedList<state>();
//    LinkedList<state>.Enumerator i;

//    if (u != s_goal)
//    {
//      getSucc(u, s);
//      double tmp = INFINITY;
//      double tmp2;

//      for (i = s.GetEnumerator(); i.MoveNext();)
//      {
//        tmp2 = getG(i.Current) + cost(u, i.Current);
//        if (tmp2 < tmp)
//        {
//            tmp = tmp2;
//        }
//      }
//      if (!close(getRHS(u), tmp))
//      {
//          setRHS(u, tmp);
//      }
//    }
//    if (!close(getG(u), getRHS(u)))
//    {
//        insert(u);
//    }

//  }

//  /* 
//   * Inserts state u into openList and openHash.
//   */
//  private void insert(state u)
//  {

//    hash_map<state, float, state_hash, equal_to<state>>.iterator cur = new hash_map<state, float, state_hash, equal_to<state>>.iterator();
//    float csum;

//    u = calculateKey(u);
//    cur = openHash.find(u);
//    csum = keyHashCode(u);
//    // return if cell is already in list. TODO: this should be
//    // uncommented except it introduces a bug, I suspect that there is a
//    // bug somewhere else and having duplicates in the openList queue
//    // hides the problem...
//    //if ((cur != openHash.end()) && (close(csum,cur->second))) return;

//    openHash[u] = csum;
//    openList.push(u);
//  }

//  /*
//   * Removes state u from openHash. The state is removed from the
//   * openList lazilily (in replan) to save computation.
//   */
//  private void remove(state u)
//  {

//    hash_map<state, float, state_hash, equal_to<state>>.iterator cur = openHash.find(u);
//    if (cur == openHash.end())
//        return;
//    openHash.erase(cur);
//  }

//  /*
//   * Euclidean cost between state a and state b.
//   */
//  private double trueDist(state a, state b)
//  {

//    float x = a.x - b.x;
//    float y = a.y - b.y;
//    return Math.Sqrt(x * x + y * y);

//  }

//  /*
//   * Pretty self explanitory, the heristic we use is the 8-way distance
//   * scaled by a constant C1 (should be set to <= min cost).
//   */
//  private double heuristic(state a, state b)
//  {
//    return eightCondist(a, b) * C1;
//  }

//  /* state Dstar::calculateKey(state u)
//   * --------------------------
//   * As per [S. Koenig, 2002]
//   */
//  private state calculateKey(state u)
//  {
//    double val = fmin(getRHS(u),getG(u));
//    u.k.first = val + heuristic(u, s_start) + k_m;
//    u.k.second = val;

//    return u;

//  }

//  /*
//   * Returns a list of successor states for state u, since this is an
//   * 8-way graph this list contains all of a cells neighbours. Unless
//   * the cell is occupied in which case it has no successors. 
//   */
//  private void getSucc(state u, LinkedList<state> s)
//  {

//    s.Clear();
//    u.k.first = -1;
//    u.k.second = -1;
//    if (occupied(u))
//        return;

//    u.x += 1;
//    s.AddFirst(u);
//    u.y += 1;
//    s.AddFirst(u);
//    u.x -= 1;
//    s.AddFirst(u);
//    u.x -= 1;
//    s.AddFirst(u);
//    u.y -= 1;
//    s.AddFirst(u);
//    u.y -= 1;
//    s.AddFirst(u);
//    u.x += 1;
//    s.AddFirst(u);
//    u.x += 1;
//    s.AddFirst(u);

//  }

//  /* 
//   * Returns a list of all the predecessor states for state u. Since
//   * this is for an 8-way connected graph the list contails all the
//   * neighbours for state u. Occupied neighbours are not added to the
//   * list.
//   */
//  private void getPred(state u, LinkedList<state> s)
//  {

//    s.Clear();
//    u.k.first = -1;
//    u.k.second = -1;

//    u.x += 1;
//    if (!occupied(u))
//    {
//        s.AddFirst(u);
//    }
//    u.y += 1;
//    if (!occupied(u))
//    {
//        s.AddFirst(u);
//    }
//    u.x -= 1;
//    if (!occupied(u))
//    {
//        s.AddFirst(u);
//    }
//    u.x -= 1;
//    if (!occupied(u))
//    {
//        s.AddFirst(u);
//    }
//    u.y -= 1;
//    if (!occupied(u))
//    {
//        s.AddFirst(u);
//    }
//    u.y -= 1;
//    if (!occupied(u))
//    {
//        s.AddFirst(u);
//    }
//    u.x += 1;
//    if (!occupied(u))
//    {
//        s.AddFirst(u);
//    }
//    u.x += 1;
//    if (!occupied(u))
//    {
//        s.AddFirst(u);
//    }

//  }

//  /*
//   * Returns the cost of moving from state a to state b. This could be
//   * either the cost of moving off state a or onto state b, we went with
//   * the former. This is also the 8-way cost.
//   */
//  private double cost(state a, state b)
//  {

//    int xd = Math.Abs(a.x - b.x);
//    int yd = Math.Abs(a.y - b.y);
//    double scale = 1;

//    if (xd + yd > 1)
//    {
//        scale = M_SQRT2;
//    }

//    if (cellHash.count(a) == 0)
//    {
//        return scale * C1;
//    }
//    return scale * cellHash[a].cost;

//  }

//  /* 
//   * returns true if the cell is occupied (non-traversable), false
//   * otherwise. non-traversable are marked with a cost < 0.
//   */
//  private bool occupied(state u)
//  {

//    hash_map<state,cellInfo, state_hash, equal_to<state>>.iterator cur = cellHash.find(u);

//    if (cur == cellHash.end())
//    {
//        return false;
//    }
//    return (cur.second.cost < 0);
//  }

//  /* 
//   * Returns true if state u is on the open list or not by checking if
//   * it is in the hash table.
//   */
//  private bool isValid(state u)
//  {

//    hash_map<state, float, state_hash, equal_to<state>>.iterator cur = openHash.find(u);
//    if (cur == openHash.end())
//    {
//        return false;
//    }
//    if (!close(keyHashCode(u), cur.second))
//    {
//        return false;
//    }
//    return true;

//  }

//  /* 
//   * Returns the key hash code for the state u, this is used to compare
//   * a state that have been updated
//   */
//  private float keyHashCode(state u)
//  {

//    return (float)(u.k.first + 1193 * u.k.second);

//  }
//}

