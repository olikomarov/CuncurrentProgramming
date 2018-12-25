#include <stdio.h>
#include <conio.h>
#include <iostream>
#include <iomanip>
#include <omp.h>
#include <list>
#include <utility>
#include <exception>


using namespace std;

struct Result
{
	double timestamp, area;
};

double f(const double x);
const Result method_rectangle(const double, const double, const double, const int);

int main()
{
	const short maxThreads = 10;
	short method;
	double x1, x2, dx;

	cout << fixed << setprecision(8) << endl;
	try
	{
		while (true)
		{
			cout << "   x1: "; 
			cin >> x1;
			cout << "   x2: "; 
			cin >> x2;
			cout << "   dx: "; 
			cin >> dx;

			list<pair<short, Result>> results;
			for (int i = 0; i < maxThreads; i++)
			{
				Result result= method_rectangle(x1, x2, dx, i + 1);
				pair<short, Result> s_result(i + 1, result);
				results.push_back(s_result);
			}

			cout << endl << "   Results:" << endl;
			for (auto & result : results)
			{
				cout << "   Threads: " << result.first;
				cout << ", timestamp: " << result.second.timestamp;
				cout << ", area: " << result.second.area << endl;
			}
			cout << endl;
		}
	}
	catch (exception & e)
	{
		cout << e.what() << endl;
	}
	cin.get();
	return 0;
}

const Result method_rectangle(const double x1, const double x2, const double dx, const int nThreads)
{
	const int N = static_cast<int>((x2 - x1) / dx);
	double now = omp_get_wtime();
	double s = 0;

#pragma omp parallel for num_threads(nThreads) reduction(+: s)
	for (int i = 1; i <= N; i++) s += f(x1 + i * dx);

	s *= dx;

	return { omp_get_wtime() - now, s };
}

double f(const double x)
{
	return sin(x);
}