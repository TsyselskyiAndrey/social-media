import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';

class PlanCard extends StatelessWidget {
  final String title;
  final String price;
  final List<String> features;
  final bool isPopular;

  const PlanCard({
    required this.title,
    required this.price,
    required this.features,
    this.isPopular = false,
    super.key,
  });

  Widget get checkSvg => SvgPicture.asset(
        'assets/svg/check.svg',
        width: 20,
        height: 20,
      );

  Widget get starsImg => Image.asset(
        'assets/images/stars.png',
        width: 30,
        height: 30,
      );

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.sizeOf(context).height * 0.35,
      margin: const EdgeInsets.all(20),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(15),
      ),
      child: LayoutBuilder(
        builder: (context, constraints) {
          return Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              Stack(
                children: [
                  Align(
                    alignment: Alignment.bottomCenter,
                    child: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        starsImg,
                        const SizedBox(width: 7),
                        Text(
                          title,
                          style: const TextStyle(
                            fontSize: 30,
                            fontWeight: FontWeight.w500,
                          ),
                        ),
                        const SizedBox(width: 7),
                        if (title == 'Elite Plan') starsImg,
                      ],
                    ),
                  ),
                  if (isPopular)
                    Padding(
                      padding: const EdgeInsets.only(right: 8.0),
                      child: Align(
                        alignment: Alignment.topRight,
                        child: Container(
                          width: constraints.maxWidth * 0.25,
                          height: constraints.minHeight * 0.09,
                          decoration: BoxDecoration(
                            color: Colors.black,
                            borderRadius: BorderRadius.circular(12),
                          ),
                          child: const Center(
                            child: Text(
                              "Popular",
                              style: TextStyle(
                                fontSize: 15,
                                color: Colors.white,
                              ),
                            ),
                          ),
                        ),
                      ),
                    ),
                ],
              ),
              Row(
                children: [
                  const Padding(
                    padding: EdgeInsets.only(left: 28.0),
                    child: Text(
                      "\$",
                      style: TextStyle(
                        fontSize: 30,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  ),
                  Text(
                    price,
                    style: const TextStyle(
                      fontSize: 30,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                  const Text(
                    " / month",
                    style: TextStyle(
                      fontSize: 23,
                      fontWeight: FontWeight.w400,
                    ),
                  ),
                ],
              ),
              SizedBox(
                width: constraints.maxWidth * 0.8,
                height: constraints.maxHeight * 0.15,
                child: TextButton(
                  onPressed: () {},
                  style: TextButton.styleFrom(
                    backgroundColor: Colors.black,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                  ),
                  child: const Text(
                    "Get Started",
                    style: TextStyle(color: Colors.white, fontSize: 18),
                  ),
                ),
              ),
              Column(
                children: features.map((feature) {
                  return Padding(
                    padding: const EdgeInsets.only(left: 30.0, bottom: 8),
                    child: Row(
                      children: [
                        checkSvg,
                        const SizedBox(width: 10),
                        Expanded(
                          child: Text(
                            feature,
                            style: const TextStyle(
                              color: Colors.black,
                              fontSize: 20,
                              fontWeight: FontWeight.w500,
                            ),
                          ),
                        ),
                      ],
                    ),
                  );
                }).toList(),
              ),
            ],
          );
        },
      ),
    );
  }
}
