import matplotlib.pyplot as plt

dataset = {
    1: {'cat': 'Weapon', 'x': 9, 'y': 18},
    2: {'cat': 'Weapon', 'x': 19, 'y': 19},
    3: {'cat': 'Weapon', 'x': 19, 'y': 13},
    4: {'cat': 'Weapon', 'x': 8, 'y': 13},
    5: {'cat': 'Weapon', 'x': 19, 'y': 6},
    6: {'cat': 'Enemy', 'x': 16, 'y': 1},
    7: {'cat': 'Enemy', 'x': 13, 'y': 8},
    8: {'cat': 'Enemy', 'x': 16, 'y': 9},
    9: {'cat': 'Enemy', 'x': 15, 'y': 5},
    10: {'cat': 'Enemy', 'x': 19, 'y': 16},
    11: {'cat': 'Potion', 'x': 8, 'y': 4},
    12: {'cat': 'Potion', 'x': 2, 'y': 10},
    13: {'cat': 'Potion', 'x': 9, 'y': 3},
    14: {'cat': 'Potion', 'x': 7, 'y': 7},
    15: {'cat': 'Potion', 'x': 10, 'y': 19}
}

centroids = {
    1: [9, 18],
    2: [16, 1],
    3: [8, 4],
    4: [10, 19]
}

clusters = {
    1: [1, 4],
    2: [5, 6, 8, 9],
    3: [7, 11, 12, 13, 14],
    4: [2, 3, 10, 15]
}

colors = {1: 'red', 2: 'green', 3: 'blue', 4: 'orange'}

def diagram(centroids, clusters, dataset):
    plt.figure(figsize=(8, 8))
    plt.title(f"Part 3 a i Iteration 1")
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.xlim(0, 20)
    plt.ylim(0, 20)
    plt.grid(True)

    for clusterId, datasetIds in clusters.items():
        clusterColor = colors.get(clusterId)

        for dataPoint in datasetIds:
             data = dataset[dataPoint]

             if data['cat'] == 'Potion':
                 plt.scatter(data['x'], data['y'], c=clusterColor, marker='s', s=100)
             elif data['cat'] == 'Weapon':
                 plt.scatter(data['x'], data['y'], c=clusterColor, marker='+', s=100)
             elif data['cat'] == 'Enemy':
                 plt.scatter(data['x'], data['y'], c=clusterColor, marker='*', s=100)
             else:
                 continue

             plt.text(data['x'] + 0.3, data['y'] + 0.3, str(dataPoint))

    for clusterId, position in centroids.items():
        plt.scatter(position[0], position[1], c="black", marker='X', s=200, alpha=0.5)
        plt.text(position[0], position[1] - 1, f"C{clusterId}")

    plt.show()

diagram(centroids, clusters, dataset)
