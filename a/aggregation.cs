public class Aggregation
{
    Point[] p;
    Circle[] c;
    Line[] l;

    public void move()
    {
        foreach (var i in p)
        {
            i.move();
        }

        foreach (var i in c)
        {
            i.move();
        }
        foreach (var i in c)
        {
            i.move();
        }
    }
    public void rotate()
    {
        foreach (var i in p)
        {
            i.rotate();
        }

        foreach (var i in c)
        {
            i.rotate();
        }
        foreach (var i in c)
        {
            i.rotate();
        }
    }
}